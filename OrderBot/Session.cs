using System;

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
            String sMessage = "Welcome to P&P Optica!\nWhat's your choice for the imaging machine?\n1. P&P Imaging AV-1290(Vegetables only)\n2. P&P Imaging AX-1583(Meat only)\n3. P&P Imaging BX-1649(Both)\n Please respond with a number based on your choice.";
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
                    this.oOrder.MachineType="P&P Imaging AX-1583";
                    else if(machinetypeNo==3)
                    this.oOrder.MachineType="P&P Imaging BX-1649";
                    else
                    {
                    sMessage="Wrong input! Please input numbers from the below options:\n1. P&P Imaging AV-1290(Vegetables only)\n2. P&P Imaging AX-1583(Meat only)\n3. P&P Imaging BX-1649(For both)";
                    this.nCur=State.MACHINETYPE;
                    break;
                    }
                    sMessage = "What food products would you like to scan using " + this.oOrder.MachineType + " imaging machine?";
                    this.nCur=State.FOODPRODUCTS;
                    break;
                case State.FOODPRODUCTS:
                    sMessage = "What is the quality criteria" + this.oOrder.MachineType+ " imaging machine ?";
                    break;
                case State.SIZE:
                    sMessage = "What is the required scan count per hour for the" + this.oOrder.MachineType+ " imaging machine ?";
                    this.nCur=State.FOODPRODUCTS;
                    break;
                
            }
            System.Diagnostics.Debug.WriteLine(sMessage);
            return sMessage;
        }

    }
}
