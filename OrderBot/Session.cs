using System;
using System.Collections.Generic;
using System.Text;

namespace OrderBot
{
    public class Session
    {
        private enum State
        {
            WELCOMING, MACHINETYPE, FOODPRODUCTS, QUALITYCRITERIA, CONTACTSERVICEAGENT, BOOKED
        }

        private State nCur = State.WELCOMING;
        private Order oOrder;

        public Session(string sPhone){
            Random random=new Random();
            this.oOrder = new Order();
            this.oOrder.AppointmentID = random.Next().ToString();
        }

        public String OnMessage(String sInMessage)
        {
            //Welcome message
            String sMessage = "Welcome to P&P Optica!\nWhat's your choice for the imaging machine?\n1. P&P Imaging AV-1290(Vegetables only)\n2. P&P Imaging AM-1583(Meat only)\n Please respond with a number based on your choice.";
            
            //Dates
            String FirstDate=DateTime.Now.AddDays(1).AddHours(12).ToString("dddd, dd MMMM yyyy hh:mm tt");
            String SecondDate=DateTime.Now.AddDays(1).AddHours(15).ToString("dddd, dd MMMM yyyy hh:mm tt");
            String ThirdDate=DateTime.Now.AddDays(2).AddHours(15).ToString("dddd, dd MMMM yyyy hh:mm tt");
            switch (this.nCur)
            {
                case State.WELCOMING:
                    this.nCur=State.MACHINETYPE;
                    break;
                case State.MACHINETYPE:
                    int machinetypeNo=0;
                    int.TryParse(sInMessage, out machinetypeNo);
                    if(machinetypeNo==1)
                    this.oOrder.MachineType="P&P Imaging AV-1290";
                    else if(machinetypeNo==2)
                    this.oOrder.MachineType="P&P Imaging AM-1583";
                    else
                    {
                    sMessage="Wrong input! Please input numbers from the below options:\n1. P&P Imaging AV-1290(Vegetables only)\n2. P&P Imaging AM-1583(Meat only)";
                    this.nCur=State.MACHINETYPE;
                    break;
                    }
                    sMessage = "What food products would you like to scan using " + this.oOrder.MachineType + " imaging machine?";
                    this.nCur=State.FOODPRODUCTS;
                    break;
                case State.FOODPRODUCTS:
                    List<string> vegetables=new List<string>(){"carrot","beetroot","turnip","tomato","cucumber","potato","radish","cabbage"};
                    List<string> meat=new List<string>(){"chicken","beef","pork","lamb","fish"};

                    if(sInMessage.ToLower()=="y"||sInMessage.ToLower()=="yes")
                    {
                            if(oOrder._foodProducts.Count==0){
                                sMessage="No food products selected!. Please enter food products.";
                                break;
                            }
                            else{
                                 StringBuilder sb = new StringBuilder();
                                foreach (String s in oOrder._foodProducts)
                                {
                                    sb.Append(" "+s);
                                }
                            sMessage = "What is the quality criteria for the selected food products\n"+sb+"\n in the "+this.oOrder.MachineType+ " imaging machine?";
                            this.nCur=State.QUALITYCRITERIA;
                            break;
                            }
                    }
                    else
                    {
                    sMessage=sInMessage+" is an invalid input!. Please enter a valid input";
                    if(oOrder.MachineType.Contains("AV-1290"))
                    {
                        string selectedproduct=SelectFoodProduct(sInMessage,vegetables);
                        if(!selectedproduct.Equals(string.Empty))
                        sMessage=selectedproduct;
                        foreach(string s in meat)
                        {
                            if(s==sInMessage.ToLower())
                            {
                                sMessage=sInMessage.ToLower()+" cannot be processed using the selected mechine "+oOrder.MachineType+".\nPlease input vegetable.";
                                break;
                            }
                        }                      
                    }
                    if(oOrder.MachineType.Contains("AM-1583"))
                    { 
                        string selectedproduct=SelectFoodProduct(sInMessage,meat);
                        if(!selectedproduct.Equals(string.Empty))
                        sMessage=selectedproduct;
                        foreach(string s in vegetables)
                        {
                            if(s==sInMessage.ToLower())
                            {
                                sMessage=sInMessage.ToLower()+" cannot be processed using the selected mechine "+oOrder.MachineType+".\nPlease input meat type.";
                                break;
                            }
                        }                      
                    }
                    }
                    break;
                case State.QUALITYCRITERIA:
                    List<string> vegetableQC=new List<string>(){"soil", "mud", "sand", "plastic", "fungus", "pesticide", "age"};
                    List<string> meatQC=new List<string>(){"fungus", "plastic", "bone", "fat", "blood clot", "hair", "age"};
                    
                    if(sInMessage.ToLower()=="y"||sInMessage.ToLower()=="yes")
                    {
                            if(oOrder._qualityCriteria.Count==0){
                                sMessage="Quality criteria not selected! Please enter quality criteria.";
                                break;
                            }
                            else{
                            //sMessage = "What is the required scan count per hour for the " + this.oOrder.MachineType+ " imaging machine ?";
                            sMessage = "You are almost close to finishing your order.\n"
                            +"Please select from the below slot for meeting with our Customer service agent \nto select size and complete the order:\n"+
                            "1. "+FirstDate+"\n2. "+ SecondDate+"\n3. "+ThirdDate+"\nPlease respond with the option number.";
                            this.nCur=State.CONTACTSERVICEAGENT;
                            break;
                            }
                    }
                    else
                    {
                    sMessage=sInMessage+" is an invalid input!. Please enter a valid input";
                    if(oOrder.MachineType.Contains("AV-1290"))
                    {
                        string selectedCriteria=SelectCriteria(sInMessage,vegetableQC);
                        if(!selectedCriteria.Equals(string.Empty))
                        sMessage=selectedCriteria;
                        foreach(string s in meatQC)
                        {
                            if(s==sInMessage.ToLower())
                            {
                                if(!sMessage.Contains("already selected")&&!sMessage.Contains("Apart")){
                                sMessage=sInMessage.ToLower()+" is not a valid quality criteria for vegetables.\nPlease input a valid criteria.";
                                break;
                                }
                            }
                        }                      
                    }
                    if(oOrder.MachineType.Contains("AM-1583"))
                    { 
                        string selectedCriteria=SelectCriteria(sInMessage,meatQC);
                        if(!selectedCriteria.Equals(string.Empty))
                        sMessage=selectedCriteria;
                        foreach(string s in vegetableQC)
                        {
                            if(s==sInMessage.ToLower())
                            {   
                                if(!sMessage.Contains("already selected")&&!sMessage.Contains("Apart"))
                                {
                                sMessage=sInMessage.ToLower()+" is not a valid quality criteria for meat.\nPlease input a valid criteria.";
                                break;
                                }
                            }
                        }                      
                    }
                    }
                    break;
                case State.CONTACTSERVICEAGENT:
                StringBuilder food = new StringBuilder();
                                foreach (String s in oOrder._foodProducts)
                                {
                                    food.Append(" "+s);
                                }
                StringBuilder quality = new StringBuilder();
                                foreach (String s in oOrder._qualityCriteria)
                                {
                                    quality.Append(" "+s);
                                }     
                int userdateselection=0;
                int.TryParse(sInMessage, out userdateselection);
                if(userdateselection!=0){
                if(userdateselection==1){
                    oOrder.AppointmentDate=FirstDate;
                    this.nCur=State.BOOKED;
                }
                else if(userdateselection==2){
                    oOrder.AppointmentDate=SecondDate;
                    this.nCur=State.BOOKED;
                }
                else if(userdateselection==3){
                    oOrder.AppointmentDate=ThirdDate;
                    this.nCur=State.BOOKED;
                }
                else{
                     sMessage="Selected date is not available. Please select from the available options displayed below.\n"+
                    "1. "+FirstDate+"\n2. "+ SecondDate+"\n3. "+ThirdDate+"\nPlease respond with the option number.";
                    break;
                }
                
                sMessage="Your appointment with our customer service agent is successfully confirmed for "+oOrder.AppointmentDate+"\n"+
                "Please find the order details below:\n"+
                "Machine Type: "+oOrder.MachineType+"\n"+
                "Products to Test: "+food+"\n"+
                "Quality criteria: "+quality;
                this.oOrder.Save();
                }
                else
                {
                   sMessage="Invalid Input. Please select from the available options displayed below.\n"+
                    "1. "+FirstDate+"\n2. "+ SecondDate+"\n3. "+ThirdDate+"\nPlease respond with the option number.";
                }
                break;
                case State.BOOKED:
                sMessage="Your appointment is confirmed for "+oOrder.AppointmentDate+"\n";
                break;
            }
            System.Diagnostics.Debug.WriteLine(sMessage);
            return sMessage;
        }

        public string SelectFoodProduct(String sInMessage, List<string> product)
        {
            string sMessage="";
            int unique=1;
            foreach(string s in product)
            {
                    if(s==sInMessage.ToLower())
                    {
                            foreach(string food in oOrder._foodProducts)
                                {
                                    if(food==sInMessage.ToLower())
                                    {
                                    sMessage=sInMessage.ToLower()+" is already selected! Please input a different product!";
                                    unique=0;
                                    break;
                                    }
                                }
                                if(unique==1){
                                oOrder._foodProducts.Add(sInMessage);                                
                                sMessage="Apart from "+string.Join(", ",oOrder._foodProducts)+"\n what other product do you want to scan using "+oOrder.MachineType+" ?\n Type \"Yes\" if you are done selecting food products.";
                                }
                    }
            }
            return sMessage;
        }
        public string SelectCriteria(string sInMessage, List<string> criteria){
            string sMessage="";
            int unique=1;
            foreach(string s in criteria)
            {
                    if(s==sInMessage.ToLower())
                    {
                            foreach(string food in oOrder._qualityCriteria)
                            {
                                if(food==sInMessage.ToLower())
                                {
                                    sMessage=sInMessage.ToLower()+" is already selected! Please input a different criteria!";
                                    unique=0;
                                    break;
                                }
                            }
                                if(unique==1)
                            {
                                oOrder._qualityCriteria.Add(sInMessage);                                
                                sMessage="Apart from "+string.Join(", ",oOrder._qualityCriteria)+"\n what other criteria do you want to scan using "+oOrder.MachineType+" ?\n Type \"Yes\" if you are done selecting quality criteria.";
                            }
                    }
            }
            return sMessage;
        }
    }
}
