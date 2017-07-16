using Braintree;
using LaboUSER.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaboUSER.Areas.user.Models;
namespace LaboUSER.Areas.user.Controllers
{
    public class PaymentController : Controller
    {
        //
        // GET: /user/checkout/
        public IBraintreeConfiguration config = new BraintreeConfiguration();
        public DataEncryptor keys = new DataEncryptor();
        public static readonly TransactionStatus[] transactionSuccessStatuses = {
                                                                                    TransactionStatus.AUTHORIZED,
                                                                                    TransactionStatus.AUTHORIZING,
                                                                                    TransactionStatus.SETTLED,
                                                                                    TransactionStatus.SETTLING,
                                                                                    TransactionStatus.SETTLEMENT_CONFIRMED,
                                                                                    TransactionStatus.SETTLEMENT_PENDING,
                                                                                    TransactionStatus.SUBMITTED_FOR_SETTLEMENT
                                                                                };
        public ActionResult checkout(string userid, string jobid)
        {
            var gateway = config.GetGateway();
            var clientToken = gateway.ClientToken.generate();
            ViewBag.ClientToken = clientToken;

            //Check for this jobid,userid,transaction Id status
            if (!string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(jobid))
            {
                userid = keys.DecryptString(userid);
                jobid = keys.DecryptString(jobid);
                try
                {
                    var table = new DataTable();
                    SqlConnection conn = new SqlConnection("Data source=148.72.232.166; Database=hardyhat;User Id=vineshnilesh888;Password=VineshNilesh88");
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("select * from [dbo].[tbl_JobPayment] where Fk_JobId=" + jobid + " and UserId=" + userid + ";", conn);
                    // create data adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // this will query your database and return the result to your datatable
                    da.Fill(table);
                    conn.Close();
                    da.Dispose();
                    if (table.Rows.Count == 0)
                    {
                        //insert into the job payment table...!!!
                        Decimal companyFee = Convert.ToDecimal("2.00");
                        conn.Open();
                        SqlCommand cmdInsert = new SqlCommand("insert into [dbo].[tbl_JobPayment] (Fk_JobId,UserId,Amount) values(" + jobid + "," + userid + "," + companyFee + ");", conn);
                        cmdInsert.ExecuteNonQuery();
                        conn.Close();
                        Console.WriteLine("Inserting Data Successfully");

                    }
                    else if (Convert.ToString(table.Rows[0]["PaymentStatus"]) == "submitted_for_settlement")
                    {
                        return RedirectToRoute("error-authorized");
                    }
                    else
                    {
                        string pk_paymentId = Convert.ToString(table.Rows[0]["Pk_JobPaymentId"]);
                        clsSession.paymentID = pk_paymentId;
                    }

                }
                catch (Exception e)
                {
                }
            }
            else
            {
                return RedirectToRoute("error-authorized");
            }
            return View();
        }

        public ActionResult Create()
        {
            var gateway = config.GetGateway();
            Decimal amount;

            try
            {
                amount = Convert.ToDecimal("2");
            }
            catch (FormatException e)
            {
                TempData["Flash"] = "Error: 81503: Amount is an invalid format.";
                return RedirectToAction("New");
            }

            var nonce = Request["payment_method_nonce"];
            var request = new TransactionRequest
            {
                Amount = amount,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            if (result.IsSuccess())
            {
                Transaction transaction = result.Target;
                return RedirectToAction("checkout");
            }
            else if (result.Transaction != null)
            {
                // return RedirectToAction("Show", new { id = result.Transaction.Id });
                return RedirectToAction("checkout");
            }
            else
            {
                string errorMessages = "";
                foreach (ValidationError error in result.Errors.DeepAll())
                {
                    errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
                }
                TempData["Flash"] = errorMessages;
                return RedirectToAction("checkout");
            }

        }

        public ActionResult Show(String id)
        {
            var gateway = config.GetGateway();
            Transaction transaction = gateway.Transaction.Find(id);
            //Update the transaction table with updated info.!!!
            try
            {
                SqlConnection conn = new SqlConnection("Data source=148.72.232.166; Database=hardyhat;User Id=vineshnilesh888;Password=VineshNilesh88");
                conn.Open();
                string status = "success";
                SqlCommand cmdUpdate = new SqlCommand("update [dbo].[tbl_JobPayment] SET TransactionId='" + id + "',PaymentStatus='" + transaction.Status + "' WHERE Pk_JobPaymentId=" + Convert.ToInt32(clsSession.paymentID) + " ", conn);
                cmdUpdate.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                //Handle the exception

            }

            if (transactionSuccessStatuses.Contains(transaction.Status))
            {
                TempData["header"] = "Sweet Success!";
                TempData["icon"] = "success";
                TempData["message"] = "Your test transaction has been successfully processed. See the API response and try again.";
            }
            else
            {
                TempData["header"] = "Transaction Failed";
                TempData["icon"] = "fail";
                TempData["message"] = "Your test transaction has a status of " + transaction.Status + ". See the API response and try again.";
            };

            ViewBag.Transaction = transaction;
            return View();
        }

    }
}
