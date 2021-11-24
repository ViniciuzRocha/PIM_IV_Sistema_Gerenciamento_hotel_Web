using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PIM_IV_DAL;
using PIM_IV_MODEL;

namespace pim_web2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult app()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult reservas()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult CadastroHospede(string nome, string cpf, string email, string celular, string cep, char sexo, string login, string senha)
        {
            HospedeDAO hospedeDAO = new HospedeDAO();
            Hospede hospede = new Hospede();
            hospede.hNome = nome;
            hospede.hCPF = cpf;
            hospede.hEmail = email;
            hospede.hCelular = celular;
            hospede.hCep = cep;
            hospede.hSexo = sexo;
            hospede.hLogin = email;
            hospede.hSenha = senha;
            hospede.hFuncionario = 1;

            hospedeDAO.adicionarhospede(hospede);
            return RedirectToAction(Url.Action("../login"));

        }
        [HttpPost]
        public ActionResult LoginHospede(string email, string senha)
        {
         
            Hospede hospede = new Hospede();
            hospede.hLogin = email;
            hospede.hSenha = senha;
            hospede.hFuncionario = 1;

            if (hospede.hLogin.Equals("admin@outlook.com") && hospede.hSenha.Equals("1234"))
            {
                return RedirectToAction(Url.Action("../Reservas"));
            }
            else
            {
                return RedirectToAction("login");
            }

        }

        [HttpPost]
        public ActionResult novareserva(DateTime entrada, DateTime saida, string radio, string cpf ) //adicionar o string cpf
        {
            ReservaDAO reservaDAO = new ReservaDAO();
            Reserva reserva = new Reserva();
            reserva.rEntrada = entrada;
            reserva.rSaida = saida;
            
            int precoCategoria = 0;
            int Diaria = int.Parse(saida.Date.Subtract(entrada.Date).TotalDays.ToString());
            switch (radio)
            {
                case "STANDART":
                    precoCategoria = 150;
                    if (entrada == saida)
                    {
                        precoCategoria *= 1;
                    }
                    else
                    {
                        precoCategoria *= Diaria;
                        
                    }
                    reserva.rValor = int.Parse(precoCategoria.ToString());
                    if (reservaDAO.BuscarQuartoLivre(1) != 0)
                    {
                        reserva.rQuarto = reservaDAO.BuscarQuartoLivre(1);
                    }
                    else
                    {
                        return RedirectToAction(Url.Action("../reservas"));

                    }
                    break;

                case "MASTER":
                    precoCategoria = 300;
                    if (entrada == saida)
                    {
                        precoCategoria *= 1;
                    }
                    else
                    {
                        precoCategoria *= Diaria;
                    }
                    reserva.rValor = int.Parse(precoCategoria.ToString());
                    if (reservaDAO.BuscarQuartoLivre(2) != 0)
                    {
                        reserva.rQuarto = reservaDAO.BuscarQuartoLivre(2);
                    }
                    else
                    {
                        return RedirectToAction(Url.Action("../reservas"));

                    }
                    break;
                case "DELUXE":
                    precoCategoria = 500;
                    if (entrada == saida)
                    {
                        precoCategoria *= 1;
                    }
                    else
                    {
                        precoCategoria *= Diaria;
                    }
                    reserva.rValor = int.Parse(precoCategoria.ToString());
                    if (reservaDAO.BuscarQuartoLivre(3) != 0)
                    {
                        reserva.rQuarto = reservaDAO.BuscarQuartoLivre(3);
                    }
                    else
                    {
                        return RedirectToAction(Url.Action("../reservas"));
                        
                    }
                    break;
            }
            reserva.rHospede = cpf;

            if (entrada.Date == DateTime.Now.Date)
            {
                reserva.rStatus = "ATIVO";
            }
            else
            {
                reserva.rStatus = "INATIVO";
            }
            
            reservaDAO.adicionarreserva(reserva);
            return RedirectToAction("ListadeReservas");
        }

        public ActionResult login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult cadastro()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ListadeReservas()
        {
            List<Reserva> listarreserva = new List<Reserva>();
            ReservaDAO reservaDAO = new ReservaDAO();
            listarreserva = reservaDAO.ListarReservasConfirmadas();
            var lista2 = listarreserva;          
            return View(listarreserva);
        }

        public ActionResult CancelarReserva(Reserva reserva)
        {
            ReservaDAO reservaDAO = new ReservaDAO();
            reserva.rStatus = "INATIVO";
            reservaDAO.alterarreserva(reserva);
            return RedirectToAction("Index");
        }
    }
}