using Gtk;
using System;
using System.IO;
 
class Hello {
    public class AboutWindow : Window {        
        public AboutWindow() : base("About") {
            VBox vbox = new VBox();
            HBox hbox = new HBox();
            Image imgLogo = new Image("162294AFWM.jpg"); 
            hbox.Add(imgLogo);
            Label lbl = new Label("Arick's Notepad\n\nChui-Wen Chiu's Note\nhttp://chuiwenchiu.spaces.live.com"); 
            hbox.Add(lbl);
            
            Add(hbox);
        }
    }

    public class MyWindow : Window {
        public MyWindow() : base("Arick's Notepad"){            
            InitComponent(); 
        }

        private Gtk.Clipboard clip ;
        private TextView txtMain;
        private void InitComponent() {
            txtMain = new TextView(new TextBuffer(new TextTagTable()));            
            txtMain.WidthRequest = 400;
            txtMain.HeightRequest = 400;

            Statusbar sb = new Statusbar();
            const int id = 1;
            sb.Push(id, "Chui-Wen Chiu's Note, http://chuiwenchiu.spaces.live.com");
            sb.HasResizeGrip = false;

            HBox hBox = new HBox();
            hBox.Add(txtMain);

 
            VBox box = new VBox();
            box.Add(InitMenu());
            box.Add(hBox);  
            box.Add(sb);
 
            Add(box);

            clip = Gtk.Clipboard.Get(Gdk.Atom.Intern("CLIPBOARD", true));
            
        }

        private MenuBar InitMenu() {
            MenuItem mnuFile = new MenuItem("File");            
            Menu mnuFileList = new Menu();

            MenuItem mnuNew = new MenuItem("New");
            mnuNew.Activated += delegate {
            };
            mnuFileList.Append(mnuNew);
  
            MenuItem mnuOpen = new MenuItem("Open");
            mnuOpen.Activated += delegate {
                Gtk.FileChooserDialog fc =
                new Gtk.FileChooserDialog("Choose the file to open",
                                            this,
                                            FileChooserAction.Open,
                                            "Cancel", ResponseType.Cancel,
                                            "Open", ResponseType.Accept);

                if (fc.Run() == (int)ResponseType.Accept) {
                    using (StreamReader sr = new StreamReader(fc.Filename)) {
                        txtMain.Buffer.Text = sr.ReadToEnd();
                    }         

                }

                fc.Destroy();

            };
            mnuFileList.Append(mnuOpen);

            MenuItem mnuSave = new MenuItem("Save");
            mnuSave.Activated += delegate {
                using (StreamWriter sw = new StreamWriter("output.txt")) {
                    sw.Write(txtMain.Buffer.Text);    
                }
            };

            mnuFileList.Append(mnuSave);

            MenuItem mnuSaveAs = new MenuItem("Save As");
            mnuSaveAs.Activated += delegate{
                Gtk.FileChooserDialog fc =
                new Gtk.FileChooserDialog("Choose the file to save",
                                            this,
                                            FileChooserAction.Save,
                                            "Cancel", ResponseType.No,
                                            "Save", ResponseType.Yes);

                if (fc.Run() == (int)ResponseType.Yes) {
                    using (StreamWriter sw = new StreamWriter(fc.Filename)) {
                        sw.Write(txtMain.Buffer.Text);                          
                    }

                }

                fc.Destroy();
            };

            mnuFileList.Append(mnuSaveAs);
            mnuFileList.Append(new SeparatorMenuItem());   
            MenuItem mnuExit = new MenuItem("Exit");
            mnuExit.Activated += delegate {
                Application.Quit(); 
            };

            mnuFileList.Append(mnuExit);

            mnuFile.Submenu = mnuFileList;

            MenuItem mnuEdit = new MenuItem("Edit");
            Menu mnuEditList = new Menu();
            MenuItem mnuCopy = new MenuItem("Copy");
            mnuCopy.Activated += delegate {
                txtMain.Buffer.CopyClipboard(clip);   
            };

            MenuItem mnuCut = new MenuItem("Cut");
            mnuCut.Activated += delegate {
                txtMain.Buffer.CutClipboard(clip, true);  
            };

            MenuItem mnuPaste = new MenuItem("Paste");
            mnuPaste.Activated += delegate {
                txtMain.Buffer.PasteClipboard(clip); 
            };

            MenuItem mnuDelete = new MenuItem("Delete");
            mnuDelete.Activated += delegate {
                txtMain.Buffer.DeleteSelection(true, true);   
            };
            mnuEditList.Append(mnuCopy);
            mnuEditList.Append(mnuCut);
            mnuEditList.Append(mnuPaste);
            mnuEditList.Append(mnuDelete);
            mnuEdit.Submenu = mnuEditList;

            MenuItem mnuHelp = new MenuItem("Help");
            Menu mnuHelpList = new Menu();
            MenuItem mnuAbout = new MenuItem("About");
            mnuAbout.Activated += delegate {
                AboutWindow aw = new AboutWindow();                
                aw.ShowAll();                
            };
            mnuHelpList.Append(mnuAbout);  
            mnuHelp.Submenu = mnuHelpList;

            MenuBar mb = new MenuBar();
            mb.Add(mnuFile);
            mb.Add(mnuEdit);
            mb.Add(mnuHelp);
 
            return mb;
        }

        protected override bool OnDeleteEvent(Gdk.Event evnt) {
            bool ret = base.OnDeleteEvent(evnt);
            Application.Quit();

            return ret;
        }
    }

    static void Main() {
        Application.Init();
                 
        Window window = new MyWindow();
        window.ShowAll();

        Application.Run();
    }
}