using System;
using System.Collections.Generic;

namespace OrderBot
{
    public class Session
    {
        private enum State
        {
            WELCOMING, MACHINETYPE, SIZE, FOODPRODUCTS, QUALITYCRITERIA, CONTACTSERVICEAGENT
        }

        private State nCur = State.WELCOMING;
        private Order oOrder;

        public Session(string sPhone){
            this.oOrder = new Order();
            this.oOrder.Phone = sPhone;
        }

        public String OnMessage(String sInMessage)
        {
            //String sMessage = "Welcome to P&P Optica!\nWhat's your choice for the imaging machine?\n1. P&P Imaging AV-1290(Vegetables only)\n2. P&P Imaging AX-1583(Meat only)\n3. P&P Imaging BX-1649(Both)\n Please respond with a number based on your choice.";
            String sMessage = "Welcome to P&P Optica!\nWhat's your choice for the imaging machine?\n1. P&P Imaging AV-1290(Vegetables only)\n2. P&P Imaging AX-1583(Meat only)\n Please respond with a number based on your choice.";
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
                    else if(machinetypeNo==3)
                    this.oOrder.MachineType="P&P Imaging BX-1649";
                    else
                    {
                    //sMessage="Wrong input! Please input numbers from the below options:\n1. P&P Imaging AV-1290(Vegetables only)\n2. P&P Imaging AM-1583(Meat only)\n3. P&P Imaging BX-1649(For both)";
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
                            sMessage = "What is the quality criteria" + this.oOrder.MachineType+ " imaging machine?";
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
                            sMessage = "What is the required scan count per hour for the " + this.oOrder.MachineType+ " imaging machine ?";
                            this.nCur=State.SIZE;
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
                case State.SIZE:
                    sMessage = "inside size";
                    this.nCur=State.FOODPRODUCTS;
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
