using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BuenosAires.Model;
[ServiceContract]

public interface IWsAnwo
{
    [OperationContract]

    Respuesta LeerTodoAnwoStockProducto();

    [OperationContract]

    Respuesta ReservarEquipoAnwo(string nroserieanwo, string reservar_si_o_no);
}
