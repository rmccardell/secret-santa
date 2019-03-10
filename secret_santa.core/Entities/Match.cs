namespace secret_santa.core.Entities
{

    /// <summary>
    /// A Generic class used in the exchange list generation algorithm
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Match<T> where T:class {

           public T Giver {get; set;}
           public T Recipient {get; set;}
           public string ErrorMessage {get; set;}

           public Match(){

           }

           public Match(T giver, T recipient){

               this.Giver = giver;
               this.Recipient = recipient;
           }
    }
    
}