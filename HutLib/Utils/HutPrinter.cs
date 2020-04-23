using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Controls;

namespace Hut
{
    public class HutPrinter
    {
        public HutPrinter()
        {
        }

        public static void print(string text)
        {
            // Create the print dialog object and set options
            PrintDialog dialog = new PrintDialog();

            dialog.PageRangeSelection = PageRangeSelection.AllPages;
            dialog.UserPageRangeEnabled = true;

            // Display the dialog. This returns true if the user presses the Print button.
            if (dialog.ShowDialog() == true)
            {
                using (PrintDocument doc = new PrintDocument())
                {
                    int charactersonpage = 0;
                    int linesonpage = 0;

                    doc.PrintPage += delegate (object sender, PrintPageEventArgs args)
                    {
                        Font font = new Font("Arial Narrow", 10);
                        args.Graphics.MeasureString(text, font, args.MarginBounds.Size, StringFormat.GenericTypographic, out charactersonpage, out linesonpage);
                        args.Graphics.DrawString(text, font, new SolidBrush(Color.Black), new RectangleF(0, 0, doc.DefaultPageSettings.PrintableArea.Width, 1100));
                        text = text.Substring(charactersonpage);
                        args.HasMorePages = (text.Length > 0);
                    };

                    try
                    {
                        doc.Print();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exception Occured While Printing", ex);
                    }
                }
            }

            /*
            PrintDialog pDialog = new PrintDialog();
            pDialog.PageRangeSelection = PageRangeSelection.AllPages;
            pDialog.UserPageRangeEnabled = true;

            Nullable<Boolean> print = pDialog.ShowDialog();
            if (print == true)
            {
                //This would be where the printjob is submitted
                pDialog.PrintVisual(TextViewText as Visual, "text");
            }

            string filepath = Environment.CurrentDirectory + "\\test.txt";
            File.WriteAllText(filepath, TextViewText.Text);

            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(filepath);
            psi.Verb = "PRINT";

            Process.Start(psi);

            //Utill.Utill.PrintText(TextViewText.Text);

            string s = TextViewText.Text;// "Cost Bla  Bla Bla.....";

            PrintDocument p = new PrintDocument();
            p.PrintPage += delegate(object sender1, PrintPageEventArgs e1)
            {
                e1.Graphics.DrawString(s, new Font("Times New Roman", 12), new SolidBrush(System.Drawing.Color.Black), new RectangleF(0, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
            };
            try
            {
                p.Print();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured While Printing", ex);
            }*/

            //Utill.Utill.PrintText(TextViewText.Text);
        }
    }
}

//namespace TPS.Editor.Model
//{
//public class TPSControlServiceCallbackHandler : IDisposable
//{
//    TPSControlClient tpsControlClient;

//    TPSEditorMainViewModel tpsMainViewModel;
//    public TPSControlServiceCallbackHandler(TPSEditorMainViewModel _tpsMainViewModel )
//    {
//        tpsMainViewModel = _tpsMainViewModel;
//    }

//    public TPSControlClient StartService(string _ip)
//    {
//        NetTcpBinding binding = new NetTcpBinding(SecurityMode.None, false);
//        InstanceContext ctx2 = new InstanceContext(this);
//        tpsControlClient = new TPSControlClient(
//            ctx2,
//            binding,
//          new EndpointAddress(string.Format("net.tcp://{0}/TPSControl", _ip)));

//        tpsControlClient.Open();
//        tpsControlClient.StartService();
//        return this.tpsControlClient;
//    }

//    public void SetServiceType(TEST_LEVEL _testLevel)
//    {
//        //OnSetServiceType(_testLevel);
//        Console.WriteLine("SetServiceType {0} ", _testLevel.ToString());
//    }

//    public void RefreshServicEGSE()
//    {
//        Console.WriteLine("Refresh!!");
//    }

//    public void BroadcastTPSEventLog(int _line, DateTime _dt, LogLevel _logLevel, string _description)
//    {
//        //OnBroadcastTPSEventLog(_dt, _logLevel, _description);
//        Console.WriteLine("TPS EventLog!! {0} {1} {2}", _dt.ToString(), _logLevel.ToString(), _description);
//        tpsMainViewModel.SetSysLog("TPS Executor", _dt, _logLevel, _description, _line);
//    }

//    public void BroadcastTPSControlStatus(TPSControlStatus _tpsControlStatus)
//    {
//        if (_tpsControlStatus == TPSControlStatus.Control)
//        {
//            if(tpsMainViewModel.CurrentTPSControlStatus == TPSControlStatus.None)
//            {
//                tpsMainViewModel.CurrentTPSControlStatus = TPSControlStatus.Release;
//            }
//            else if (tpsMainViewModel.CurrentTPSControlStatus == TPSControlStatus.Control)
//            {
//                tpsMainViewModel.CurrentTPSControlStatus = TPSControlStatus.Control;
//            }
//        }
//        else if (_tpsControlStatus == TPSControlStatus.Release)
//        {
//            if (tpsMainViewModel.CurrentTPSControlStatus == TPSControlStatus.Release || tpsMainViewModel.CurrentTPSControlStatus == TPSControlStatus.Control)
//            {
//                tpsMainViewModel.CurrentTPSControlStatus = TPSControlStatus.None;
//            }
//        }
//    }

//    public void BroadcastSettingTPSFile( string _filePath )
//    {
//        if (tpsMainViewModel.CurrentTPSControlStatus == TPSControlStatus.Control) return;
//    }

//    public void BroadcastSettingTPSID( string _tpsID )
//    {
//        tpsMainViewModel.SetTPSOpenToID(_tpsID, -1);
//    }

//    public void BroadcastNSRun(string _userName, int _slotNum, int _rowNum, RunStatus _runStatus )
//    {
//        TPSNonScriptView NSC = tpsMainViewModel.CodeManager[_slotNum] as TPSNonScriptView;
//        NSC.setHighlight(_rowNum);

//        tpsMainViewModel.UpdateSlotUI(_slotNum, _rowNum, _runStatus);
//    }

//    public void BroadcastScriptBreakLine(int _slotNum, int _line)
//    {
//        Console.WriteLine("TPS Executor sciprt status : slot - {0}, line - {1}", _slotNum.ToString(), _line.ToString());

//        TPSScriptView SC = tpsMainViewModel.CodeManager[_slotNum] as TPSScriptView;

//        SC.SetBreakPoint(_line);
//    }

//    public void BroadcastScriptExecuteLine(int _slotNum, int _line, RunStatus _runStatus)
//    {
//        Console.WriteLine("TPS Executor sciprt status : slot - {0}, line - {1}", _slotNum.ToString(), _line.ToString());

//        TPSScriptView SC = tpsMainViewModel.CodeManager[_slotNum] as TPSScriptView;
//        SC.MoveLine(_line);

//        tpsMainViewModel.UpdateSlotUI(_slotNum, _line, _runStatus);
//    }

//    public void BroadcastCallTPS( string _tpsID, int _slotNum, RunStatus _runStatus  )
//    {
//        Console.WriteLine(string.Format("BroadcastCallTPS tpsID : {0}, SlotNum : {1}, Status : {2} ", _tpsID, _slotNum, _runStatus));
//        tpsMainViewModel.SettingScriptUI(_tpsID, _slotNum);
//    }

//    public void BroadcastRemoveTPS(string _tpsID, int _slotNum, RunStatus _runStatus)
//    {
//        Console.WriteLine(string.Format("BroadcastRemoveTPS tpsID : {0}, SlotNum : {1}, Status : {2} ", _tpsID, _slotNum, _runStatus));
//        tpsMainViewModel.RemoveSlotUI(_tpsID, _slotNum);

//    }

//    public void BroadcastCheckTM( string _mnemonic, string _tmValue )
//    {
//        tpsMainViewModel.SetSysLog("CheckTelmetry", DateTime.UtcNow, 0, string.Format("{0} : {1}", _mnemonic, _tmValue ) );
//    }

//    public void BroadcastACK(DateTime _ackDateTime, int _slot, int _line, bool _ack)
//    {
//        TPSScriptView SC = tpsMainViewModel.CodeManager[_slot] as TPSScriptView;
//        SC.SetReport(_line, _ackDateTime, _ack);
//    }

//    public void Dispose( )
//    {
//        Dispose(true);
//        GC.SuppressFinalize(this);
//    }

//    protected virtual void Dispose(bool disposing)
//    {
//        if (disposing)
//        {
//            if (tpsControlClient != null)
//            {
//                tpsControlClient.Close();
//                tpsControlClient = null;
//            }
//        }
//    }
//}
//}