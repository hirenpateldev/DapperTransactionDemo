using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DapperDemo.Models;
using System.Transactions;

namespace DapperDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string strConnStr = ConfigurationManager.ConnectionStrings["HirenTestConnectionStr"].ConnectionString;

            //Dapper test - Starts
            using (var connection = new SqlConnection(strConnStr))
            {
                connection.Open();

                //                //insert
                //                connection.Execute("insert into Person (Name,Address,PhoneNo) values(@Name,@Address,@PhoneNo)", param: new { Name = "TestPerson", Address = "TestAddress", PhoneNo = "TestPhoneNo" });
                //
                //
                //                //bulk insert test
                //                connection.Execute("insert into Person (Name,Address,PhoneNo) values(@Name,@Address,@PhoneNo)", param: new List<Person> { 
                //                    new Person{ Name="BulkPerson1", PhoneNo = "BulkPhoneNo", Address="BulkAddress" },
                //                    new Person{ Name="BulkPerson2", PhoneNo = "BulkPhoneNo", Address="BulkAddress" },
                //                    new Person{ Name="BulkPerson3", PhoneNo = "BulkPhoneNo", Address="BulkAddress" },
                //                    new Person{ Name="BulkPerson4", PhoneNo = "BulkPhoneNo", Address="BulkAddress" }
                //                });


                //Get Data from person
                var persons = connection.Query<Person>("select * from Person");

                //Get Data From Person Note
                var personNote = connection.Query<PersonNote>("select * from PersonNote");

                //Get Data From storedProcedure
                var PersonNoteFrmStoredProc = connection.Query<PersonNote>("[GetPersonNote]", param: new { strSearch = "note 1" }, commandType: CommandType.StoredProcedure);


                //Join Example

                /*
                 var sql =  @"select * from #Posts p 
                        left join #Users u on u.Id = p.OwnerId 
                        Order by p.Id";
 
                        var data = connection.Query<Post, User, Post>(sql, (post, user) => { post.Owner = user; return post;});
                        var post = data.First();
                 */

                // var sql = @"select * from PersonNote as pn inner join Person as p on pn.PersonID = pn.ID";
                // var joinPersonNote = connection.Query<Person, PersonNote, PersonNote>(sql, (per, perNot) => { perNot.person = per; return perNot; });
                // var joinPersonNote1 = connection.Query<Person, PersonNote, PersonNote>(sql,splitOn: "PersonID" );



                //Transaction with dapper starts

                try
                {
                
                    using (var transactionScope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        connection.EnlistTransaction(Transaction.Current);

                        for (int i = 0; i < 10; i++)
                        {
                            try
                            {
                                connection.Execute("insert into Person (Name,Address,PhoneNo) values(@Name,@Address,@PhoneNo)", param: new { Name = "TestTransaction" + i.ToString(), Address = "TestAddress", PhoneNo = "TestPhoneNo" });

                                //if i == 7 raise error
                                if (i == 7)
                                {
                                    throw new Exception("Erorr.... Can't go ahead...");
                                }
                            }
                            catch (Exception)
                            {
                                Transaction.Current.Rollback();
                                break;
                            }
                        }
                        transactionScope.Complete();
                        transactionScope.Dispose();

                    }
                }
                catch (TransactionAbortedException ex)
                { }
                catch (Exception)
                { }


                //Transaction with dapper ends




            }


            //Dapper test - Ends



            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
