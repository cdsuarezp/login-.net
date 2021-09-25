using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Login_prueba.Models;

namespace Login_prueba.Controllers
{
    public class HomeController : Controller
    {

        // Metodo que transforma la contraseña y la retorna a MD5

        public string MD5From(string text)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] data = Encoding.ASCII.GetBytes(text);
                byte[] hash = md5.ComputeHash(data);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        // mensanje para saber si los datos son correctos o no
        public ActionResult Index(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }


        // metodo que valida en la BD si el usuario es correcto
    public ActionResult Login(string usuario, string password)
        {

            if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(password))
            {
                DataBase db = new DataBase();

                var passMD5 = MD5From(password);

                var user = db.login.FirstOrDefault(e => e.Usuario == usuario && e.Password == passMD5);




                //usuario encontrado
                if (user != null)
                {
                    
                    var userLogin = db.Usuarios.FirstOrDefault(e => e.ID_usuario == user.ID_usuario);

                    return RedirectToAction("Index", new { message = "bienvenido " + userLogin.Nombre + " "+ userLogin.Apellido });
                   
                }
                // usuario no encontrado o datos incorrectos
                else
                {
                    return RedirectToAction("Index", new { message = "Datos incorrectos" });
                    
                }

            }
            // valida que se registren datos en el formulario
            else
            {
                return RedirectToAction("Index", new { message = "Por favor ingresa los datos" });
                
            }          
        }   
    }
}