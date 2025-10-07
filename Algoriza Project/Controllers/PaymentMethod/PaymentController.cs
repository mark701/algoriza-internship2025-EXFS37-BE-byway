using Domain.Models.DataBase.AdminPersona;
using Domain.Models.DataBase.PaymentMethod;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Repository.Interface;

namespace Algoriza_Project.Controllers.PaymentMethod
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("SaveCreditCard")]
        public async Task<IActionResult> SaveCreditCard(CreditCardPayment creditCard)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {

                var Data = await _unitOfWork.CreditCardPayment.Add(creditCard);
                await _unitOfWork.SaveChangesAsync();

                return Ok(Data);
            }
            catch (SqlException ex)
            {

                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPost("SavePaypalPayment")]
        public async Task<IActionResult> Save(PaypalPayment paypal)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {

                var Data = await _unitOfWork.PaypalPayment.Add(paypal);
                await _unitOfWork.SaveChangesAsync();

                return Ok(Data);
            }
            catch (SqlException ex)
            {

                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {

                var Data = await _unitOfWork.CreditCardPayment.GetAll();

                return Ok(Data);
            }
            catch (SqlException ex)
            {

                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
