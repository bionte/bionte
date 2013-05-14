using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Reflection;

using Bionte.Toolbox;

namespace Bionte.Entities
{
   
    abstract public class EntityCredential
    {
        public string Identifier;
        public string Secret;
    }

    abstract public class EntityAgent
    {
        public string AgentID;
        
        public ClaimsPrincipal AgentClaimsPrincipal;
        public EntityCredential Credential;
    }

    abstract public class EntitySystem
    {
        public string SistemID;
        public string Name;
        public List<EntityAgent> AutorizedAgents;
    }

    //--------------------------------
    public class EntityPerson
    {

    }

    //--------------------------------
    public class EntityStore
    {
        public string StoreID { get; set; }
        public string Name { get; set; }
        public List<EntityController> ListOfControllers;
    }

    public class EntityController
    {
        public string ControllerID { get; set; }
        public string Name { get; set; }
        public List<EntityTerminal> ListOfTerminals;
    }

    public class EntityTerminal
    {
        public string TreminalID { get; set; }
        public string Name { get; set; }
    }

    public class EntityHeaderVTOLListener
    {
        public Byte[] MessageLength = new byte[2] {0, 0}; //PD
        public Byte[] VTOLVersion = new byte[2] {0, 0}; //EBCDIC
        public Byte[] BussinesFormat = new byte[2] {0, 0}; //EBCDIC
        public Byte[] StoreNumber = new byte[5] {0, 0, 0, 0, 0}; //EBCDIC
        public Byte[] Filler = new byte[2] {0x0, 0x0}; //HEX 
    }

    public class EntityPOSToHost
    {
        public Byte[] TeminalNumber = new Byte[2] { 0, 0 }; //?
        public Byte[] MesageType = new Byte[1] {0}; //EBCDIC
        public Byte[] Flag1 = new Byte[3] { 0, 0, 0 }; //PD
        public Byte[] MessageSequence = new Byte[2] { 0, 0}; 
        public Byte[] TransactionCode = new Byte[1] {0}; 
        public Byte[] StatusC = new Byte[] {0}; 
        public Byte[] RegularAmount = new Byte[4] { 0, 0, 0, 0 }; //EBCDIC
        public Byte[] ACCReference = new Byte[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //EBCDIC
        public Byte[] InfoGeneralTerms = new Byte[15] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public Byte[] InfoTrack2 = new Byte[38] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                                                  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                                                  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                                                  0, 0, 0, 0, 0, 0, 0, 0};
        public Byte[] InfoTrack3 = new Byte[103] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                   0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                   0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                   0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                   0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                   0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                   0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                   0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                   0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                   0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                   0, 0, 0}; 
        public Byte[] DebitSequence = new Byte[1] { 0}; 
        public Byte[] TicketSequence= new Byte[2] { 0, 0 };

    }

    public  class EntityPOSHostMessage
    {
        public EntityHeaderVTOLListener HeaderVTOLListener;
        public EntityPOSToHost POSToHost;
        public IPEndPoint VTOLEndPoint;
        public Socket VTOLSocket;

        public EntityPOSHostMessage()
        {
            this.EntityPOSHostMessage1(GetVTOLEndPoint(), new EntityHeaderVTOLListener(), new EntityPOSToHost());
        }

        public void EntityPOSHostMessage1(IPEndPoint VTOLEndPoint, EntityHeaderVTOLListener HeaderVTOLListener, EntityPOSToHost POSToHost)
        {
            if ((POSToHost != null) || (HeaderVTOLListener != null) || (VTOLEndPoint != null))
            {
                this.HeaderVTOLListener = HeaderVTOLListener;
                this.POSToHost = POSToHost;
                this.VTOLEndPoint = VTOLEndPoint;

                try
                {
                    VTOLSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    VTOLSocket.SendTimeout = 10000;
                    VTOLSocket.ReceiveTimeout = 10000;

                }
                catch (SocketException se)
                {
                    throw new Exception("Can't create sockets for communicating with VTOL Service", se);
                }

                try
                {
                    //Validate socket connection as shown:
                    //http://msdn.microsoft.com/en-us/library/ych8bz3x.aspx
                    VTOLSocket.Connect(VTOLEndPoint);
                }
                catch (SocketException e)
                {
                    throw new Exception ("Error connecting VTOL Socket", e);
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
        
        public  void Send()
        {

            MemoryStream ms = new MemoryStream();
            ms = this.ToMemoryStream();
 
            Byte[] Data = new byte[ms.Length];
            ms.Read(Data, 0, (int)ms.Length);
            ms.Close();

            int sent = 0;  
            do
            {
                try
                {
                    sent += VTOLSocket.Send(Data, 0 + sent, Data.Length - sent, SocketFlags.None);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.WouldBlock ||
                        ex.SocketErrorCode == SocketError.IOPending ||
                        ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                    {
                        // socket buffer is probably full, wait and try again
                        Thread.Sleep(30);
                    }
                    else
                        throw ex;  // any serious error occurr
                }
            } while (sent < Data.Length);

        }

        public  IPEndPoint GetVTOLEndPoint()
        {
            IPEndPoint EP = new IPEndPoint(new IPAddress(new Byte[]{172,16,200,13}),3035);
            return EP;
        }

        public  MemoryStream ToMemoryStream()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            
            bw.Write(this.HeaderVTOLListener.MessageLength);
            bw.Write(this.HeaderVTOLListener.VTOLVersion);
            bw.Write(this.HeaderVTOLListener.BussinesFormat);
            bw.Write(this.HeaderVTOLListener.StoreNumber);
            bw.Write(this.HeaderVTOLListener.Filler);
            bw.Write(this.POSToHost.TeminalNumber);
            bw.Write(this.POSToHost.MesageType);
            bw.Write(this.POSToHost.Flag1);
            bw.Write(this.POSToHost.MessageSequence);
            bw.Write(this.POSToHost.TransactionCode);
            bw.Write(this.POSToHost.StatusC);
            bw.Write(this.POSToHost.RegularAmount);
            bw.Write(this.POSToHost.ACCReference);
            bw.Write(this.POSToHost.InfoGeneralTerms);
            bw.Write(this.POSToHost.InfoTrack2);
            bw.Write(this.POSToHost.InfoTrack3);
            bw.Write(this.POSToHost.DebitSequence);
            bw.Write(this.POSToHost.TicketSequence);
            
            return ms;
        }
    }
}
