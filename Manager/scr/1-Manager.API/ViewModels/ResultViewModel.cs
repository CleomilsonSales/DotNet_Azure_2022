namespace Manager.API.ViewModels{
    public class ResultViewModel{
        public string Message {get; set;}
        public bool Success {get; set;}
        //dynamic é tipo primitivo flexivel, que se torna o que ele recebe, tipo: se receber string ele sera string, se boleano se torna boleano e etc
        public dynamic Data {get; set;}
    }
}