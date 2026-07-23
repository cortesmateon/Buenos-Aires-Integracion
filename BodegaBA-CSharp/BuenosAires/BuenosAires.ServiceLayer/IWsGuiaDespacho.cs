using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BuenosAires.Model;

[ServiceContract]

public interface IWsGuiaDespacho
{
    [OperationContract]
    Respuesta ConsultarGuiaDespacho();

    [OperationContract]
    Respuesta ActualizarEstadoGuiaDespacho(int nrogd, string estadogd);
}
