using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TextEditor
{
    interface EditorEventListener 
{
	void fileNew(SimpleForm frame);
	void fileOpen(SimpleForm frame);
	void fileSave(SimpleForm frame);
	void fileSaveAs(SimpleForm frame);
	void exit(SimpleForm frame);
}

    interface EditorKeyEventListener
    {
        void KeyDown(KeyEventArgs e);
        void KeyPress(KeyEventArgs e);
        void KeyUp(KeyEventArgs e);
    }
    /*
    interface DocListener
    {
    	void changedUpdate(DocumentEvent e, MyPanelTextArea text);
    	void insertUpdate(DocumentEvent e, MyPanelTextArea text);
    	void removeUpdate(DocumentEvent e, MyPanelTextArea text);
    }

    interface EditorWindowListener
    {
    	void Activated(WindowEvent e) ;
    	void Closed(WindowEvent e) ;
    	void Closing(WindowEvent e) ;
    	void Deactivated(WindowEvent e) ;
    	void Deiconified(WindowEvent e) ;
    	void Iconified(WindowEvent e) ;
    	void Opened(WindowEvent e) ;
    }
    */
    public partial class SimpleForm : Form
    {
        private bool isFileNameSetted = false;
        private String fileName = "temp.tmp";
        private String tempFileName = "temp.tmp";
        private String temp = null;
        int caretTextPosition = 0;
        private bool isChanged = false;
        private Timer autosave;
        private String docText = "";
        private actionsHistory list;
        private bool Action = true;

        public SimpleForm()
        {
            InitializeComponent();
        }

        private void TextArea_KeyDown(object sender, KeyEventArgs e)
        {
            new keyEvents(this).KeyDown(e);
        }

        public bool getIsFileNameSetted()
        {
            return isFileNameSetted;
        }
        public void setIsFileNameSetted(bool isFileNameSetted)
        {
            this.isFileNameSetted = isFileNameSetted;
        }
        public bool getIsChanged()
        {
            return isChanged;
        }
        public void setIsChanged(bool b)
        {
            isChanged = b;
        }
        public actionsHistory getList()
        {
            return list;
        }

        public String getDocText()
        {
            return docText;
        }

        public void setDocText(String docText)
        {
            this.docText = docText;
        }
        public void setAction(bool b)
        {
            Action = b;
        }
        public bool getAction()
        {
            return Action;
        }
        public void setFileName(string str)
        {
            fileName = str;
        }
        public string getFileName()
        {
            return fileName;
        }
        public string getTempFileName()
        {
            return tempFileName;
        }

        private void TextArea_TextChanged(object sender, EventArgs e)
        {
            
        }

        
    }

    class fileEvents
    {
        public void fileNew(SimpleForm frame) 
	    {
		    frame.setIsFileNameSetted(false);
            frame.Text = "Text Editor";
		    
		    if(frame.getIsChanged()) 
		    {
		    	DialogResult selection = MessageBox.Show("Do you want save document?", "Warrning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
		       	if(selection == DialogResult.No)	
			    {
                    frame.setTextArea("");
			    	frame.setIsChanged(false);
			    	frame.Text = "Text Editor";
			    }
			
			    if(selection == DialogResult.Yes)
			    {
                    SaveFileDialog chooser = new SaveFileDialog();
                    if(chooser.ShowDialog() == DialogResult.OK)
                    {
                        frame.setFileName(chooser.FileName);
					    try 
					    {
					    	StreamWriter outStream = new StreamWriter(new FileStream(frame.getFileName(),FileMode.OpenOrCreate, FileAccess.Write));
					    	String str = frame.getTextArea();
					    	outStream.Write(str);
					    	outStream.Close();
					    } 
					    catch (IOException e) 
					    {
					    	
					    }
				    }
				    frame.setTextArea("");
				    frame.setIsChanged(false);
				    frame.Name = "Text Editor";
			    }
		    }
		    else
		    {
			    frame.setTextArea("");
		    }
	    }
	    public void fileOpen(SimpleForm frame) 
	    {	    		
	    	if(frame.getIsChanged())
	    	{
	    		DialogResult selection = MessageBox.Show("Do you want save document?", "Warrning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
	    		
	    		if(selection == DialogResult.No)	
	    		{
	    			OpenFileDialog chooser = new OpenFileDialog();
	    			if(chooser.ShowDialog() == DialogResult.OK)
	    			{
	    				frame.setFileName(chooser.FileName);
	    				String str = "";
	    				try 
	    				{
	    					StreamReader inStream = new StreamReader(new FileStream(frame.getFileName(),FileMode.OpenOrCreate, FileAccess.Read));
	    					while (inStream.Peek() != -1)
	    					{
	    						str = inStream.ReadToEnd();
	    					}
	    					frame.setTextArea(str);
	    				} 
	    				catch (FileNotFoundException e) 
	    				{
	    				
	    				} 
	    				catch (IOException e) 
	    				{
	    					
	    				}
	    			}
	    		}
	    		if(selection == DialogResult.Yes)
	    		{
	    			SaveFileDialog chooser = new SaveFileDialog();
	    			if(chooser.ShowDialog() == DialogResult.OK)
	    			{
                        frame.setFileName(chooser.FileName);
	    				try 
	    				{
	    					StreamWriter outStream = new StreamWriter(new FileStream(frame.getFileName(),FileMode.OpenOrCreate, FileAccess.Write));
					    	String str = frame.getTextArea();
					    	outStream.Write(str);
					    	outStream.Close();
	    				} 
	    				catch (IOException e) 
	    				{
	    					
	    				}
	    			}
	    			OpenFileDialog chooser2 = new OpenFileDialog();	
	    			if (chooser2.ShowDialog() == DialogResult.OK)
	    			{
	    				frame.setFileName(chooser2.FileName);
	    				String str = "";
	    				try 
	    				{
	    					StreamReader inStream = new StreamReader(new FileStream(frame.getFileName(),FileMode.OpenOrCreate, FileAccess.Read));
	    					while (inStream.Peek() != -1)
	    					{
	    						str = inStream.ReadToEnd();
	    					}
	    					frame.setTextArea(str);
	    				} 
	    				catch (FileNotFoundException e) 
	    				{
	    					
	    				} 
	    				catch (IOException e) 
	    				{
	    					
	    				}
	    			}
	    		}
	    	}
	    	else
	    	{
                OpenFileDialog chooser2 = new OpenFileDialog();
                if (chooser2.ShowDialog() == DialogResult.OK)
                {
                    frame.setFileName(chooser2.FileName);
                    String str = "";
                    try
                    {
                        StreamReader inStream = new StreamReader(new FileStream(frame.getFileName(), FileMode.OpenOrCreate, FileAccess.Read));
                        while (inStream.Peek() != -1)
                        {
                            str = inStream.ReadToEnd();
                        }
                        frame.setTextArea(str);
                    }
                    catch (FileNotFoundException e)
                    {

                    }
                    catch (IOException e)
                    {

                    }
                }
	    	}
	    	frame.setIsFileNameSetted(true);
	    	frame.Name = "Text Editor " + frame.getFileName();
	    }
	    public void fileSave(SimpleForm frame)
	    {
	    	if(!frame.getIsFileNameSetted())
	    	{
	    		SaveFileDialog chooser = new SaveFileDialog();
	    		if(chooser.ShowDialog() == DialogResult.OK)
	    		{
                    frame.setFileName(chooser.FileName);
	    			try 
	    			{
	    				StreamWriter outStream = new StreamWriter(new FileStream(frame.getFileName(),FileMode.OpenOrCreate, FileAccess.Write));
				    	String str = frame.getTextArea();
				    	outStream.Write(str);
				    	outStream.Close();
	    			} 
	    			catch (IOException e) 
	    			{
	    			
	    			}
	    		}
	    	}
	    	else
	    	{
                try
                {
                  StreamWriter outStream = new StreamWriter(new FileStream(frame.getFileName(), FileMode.OpenOrCreate, FileAccess.Write));
                  String str = frame.getTextArea();
                  outStream.Write(str);
                  outStream.Close();
                }
                catch (IOException e)
                {

                }
	    	}
	    	frame.setIsFileNameSetted(true);
	    	frame.Name = "Text Editor - " + frame.getFileName();
	    }
	    public void fileSaveAs(SimpleForm frame) 
	    {
            SaveFileDialog chooser = new SaveFileDialog();
            if (chooser.ShowDialog() == DialogResult.OK)
            {
                frame.setFileName(chooser.FileName);
                try
                {
                    StreamWriter outStream = new StreamWriter(new FileStream(frame.getFileName(), FileMode.OpenOrCreate, FileAccess.Write));
                    String str = frame.getTextArea();
                    outStream.Write(str);
                    outStream.Close();
                }
                catch (IOException e)
                {

                }
            }

	    	frame.setIsFileNameSetted(true);
	    	frame.Name = "Text Editor - " + frame.getFileName();
	    }
	    public void exit(SimpleForm frame) 
	    {	    		
	    	if(frame.getIsChanged()) 
	    	{
	    		DialogResult selection = MessageBox.Show("Do you want save document?", "Warrning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
	    		
	    		if(selection == DialogResult.No)	
	    		{
                    Application.Exit();
	    		}
	    			
	    		if(selection == DialogResult.Yes)
	    		{
                    SaveFileDialog chooser = new SaveFileDialog();
                    if (chooser.ShowDialog() == DialogResult.OK)
                    {
                        frame.setFileName(chooser.FileName);
                        try
                        {
                            StreamWriter outStream = new StreamWriter(new FileStream(frame.getFileName(), FileMode.OpenOrCreate, FileAccess.Write));
                            String str = frame.getTextArea();
                            outStream.Write(str);
                            outStream.Close();
                        }
                        catch (IOException e)
                        {

                        }
                    }
	    			else
	    			{
	    				return;
	    			}
	    		}
	    	}
	    	else
	    	{
                Application.Exit();
	    	}
	    		
	    }
    }

    class actionsHistory
    {
        private ArrayList actionsList;
        public static enum action { ADD, SUB };
        private int indexAction = 0;
        private SimpleForm mainFrame;

        actionsHistory(SimpleForm frame)
        {
            mainFrame = frame;
            actionsList = new ArrayList();
        }

        public void newPutString(String str, int position, action type)
        {
            if (indexAction > 0)
            {
                while (actionsList.Capacity != indexAction)
                {
                    actionsList.Remove(actionsList.Capacity - 1);
                }
            }

            actionsList.Add(new actionText(str, position, type));
            setIndexAction(indexAction + 1);
            mainFrame.setIsChanged(true);
            mainFrame.Name = "Text Editor - " + mainFrame.getFileName() + " *";
        }
        public void newGetString()
        {
            if (indexAction <= 0) return;
            actionText a = (actionText) actionsList[indexAction - 1];
            String temp = mainFrame.getTextArea();
            indexAction--;
            mainFrame.setIsChanged(true);
            mainFrame.Name = "Text Editor - " + mainFrame.getFileName() + " *";
            if (a.actionType == action.ADD)
            {
                if (a.position == 0)
                {
                    mainFrame.setTextArea(temp.Substring(a.str.Length, temp.Length));
                    mainFrame.getTextBox().SelectionStart = a.position;
                    return;
                }
                if (a.position > 0 && a.position < temp.Length)
                {
                    mainFrame.setTextArea(temp.Substring(0, a.position) + temp.Substring(a.position + a.str.Length, temp.Length));
                    mainFrame.getTextBox().SelectionStart = a.position;
                    return;
                }
                if (a.position == temp.Length)
                {
                    mainFrame.setTextArea(temp.Substring(0, a.position));
                    mainFrame.getTextBox().SelectionStart = a.position;
                    return;
                }
            }
            if (a.actionType == action.SUB)
            {
                if (a.position == 0)
                {
                    mainFrame.setTextArea(a.str + temp);
                    mainFrame.getTextBox().SelectionStart = a.position + a.str.Length;
                    return;
                }
                if (a.position > 0 && a.position < temp.Length)
                {
                    mainFrame.setTextArea(temp.Substring(0, a.position) + a.str + temp.Substring(a.position, temp.Length));
                    mainFrame.getTextBox().SelectionStart = a.position + a.str.Length;
                    return;
                }
                if (a.position == temp.Length)
                {
                    mainFrame.setTextArea(temp + a.str);
                    mainFrame.getTextBox().SelectionStart = a.position + a.str.Length;
                    return;
                }
            }
        }
        public void newRestoreString()
        {
            if (indexAction > actionsList.Capacity - 1) return;
            actionText a = (actionText)actionsList[indexAction];
            String temp = mainFrame.getTextArea();
            indexAction++;
            mainFrame.setIsChanged(true);
            mainFrame.Name = "Text Editor - " + mainFrame.getFileName() + " *";
            if (a.actionType == action.ADD)
            {
                if (a.position == 0)
                {
                    mainFrame.setTextArea(a.str + temp);
                    mainFrame.getTextBox().SelectionStart = a.position + a.str.Length;
                    return;
                }
                if (a.position > 0 && a.position < temp.Length)
                {
                    mainFrame.setTextArea(temp.Substring(0, a.position));
                    mainFrame.getTextBox().SelectionStart = a.position + a.str.Length;
                    return;
                }
                if (a.position == temp.Length)
                {
                    mainFrame.setTextArea(temp + a.str);
                    mainFrame.getTextBox().SelectionStart = a.position + a.str.Length;
                    return;
                }
            }
            if (a.actionType == action.SUB)
            {
                if (a.position == 0)
                {
                    mainFrame.setTextArea(temp.Substring(a.str.Length, temp.Length));
                    mainFrame.getTextBox().SelectionStart = a.position;
                    return;
                }
                if (a.position > 0 && a.position < temp.Length)
                {
                    mainFrame.setTextArea(temp.Substring(0, a.position) + temp.Substring(a.position + a.str.Length, temp.Length));
                    mainFrame.getTextBox().SelectionStart = a.position;
                    return;
                }
                if (a.position == temp.Length)
                {
                    mainFrame.setTextArea(temp.Substring(0, a.position));
                    mainFrame.getTextBox().SelectionStart = a.position;
                    return;
                }
            }
        }
        public ArrayList getActionList()
        {
            return actionsList;
        }
        public int getIndexAction()
        {
            return indexAction;
        }
        public void setIndexAction(int indexAction)
        {
            this.indexAction = indexAction;
        }
        class actionText
        {
            public String str;
            public int position;
            public action actionType;
            public actionText(String text, int pos, action type)
            {
                str = text;
                position = pos;
                actionType = type;
            }
        }
    }

    class keyEvents : EditorKeyEventListener
    {
    	protected SimpleForm sframe;

    	public keyEvents(SimpleForm mFrame)
    	{
    		sframe = mFrame;
    	}
        public void KeyDown(KeyEventArgs e)
    	{
    		if(e.Control && e.KeyCode == Keys.N)
    		{
    			new fileEvents().fileNew(sframe);
    		}
    		if(e.Control && e.KeyCode == Keys.O)
    		{
    			new fileEvents().fileOpen(sframe);
    		}
    		if(e.Control && e.KeyCode == Keys.S)
    		{
    			new fileEvents().fileSave(sframe);
    		}
            if (e.Control && e.Shift && e.KeyCode == Keys.S)
    		{
    			new fileEvents().fileSaveAs(sframe);
    		}
    		if(e.Control && e.KeyCode == Keys.Z)
    		{
    			//frame.getMyPanelTextArea().setAction(false);
    			new keyActions().undo(sframe);
    			//frame.getMyPanelTextArea().setAction(true);
    		}
    		if(e.Control && e.KeyCode == Keys.Y)
    		{
    			//frame.getMyPanelTextArea().setAction(false);
    			new keyActions().redo(sframe);
    			//frame.getMyPanelTextArea().setAction(true);
    		}
    		
    	}
        public void KeyPress(KeyEventArgs e)
	{
		
	}
        public void KeyUp(KeyEventArgs e)
	{
		
	}
    }

    class keyActions
    {
        public void undo(SimpleForm form)
        {
            form.getList().newGetString();
        }
        public void redo(SimpleForm form)
        {
            form.getList().newRestoreString();
        }
        public void copy(SimpleForm form)
        {
            if (form.getTextBox().SelectedText.Length != 0)
            {
                string stringSelection = form.getTextBox().SelectedText;
                Clipboard.SetText(stringSelection);
            }
        }
        public void cut(SimpleForm form)
        {
            int cp;
            if (form.getTextBox().SelectedText.Length != 0)
            {
                copy(form);
                cp = form.getTextBox().SelectionStart;
                form.getList().newPutString(form.getTextBox().SelectedText, cp, actionsHistory.action.SUB);
                if (form.getTextBox().SelectionStart == 0)
                {
                    form.setTextArea(form.getTextArea().Substring(form.getTextBox().SelectedText.Length, form.getTextArea().Length));
                    form.getTextBox().SelectionStart = cp;
                    return;
                }
                if (form.getTextBox().SelectionStart > 0 && form.getTextBox().SelectionStart < form.getTextArea().Length)
                {
                    form.setTextArea(form.getTextArea().Substring(0, form.getTextBox().SelectionStart) + form.getTextArea().Substring(form.getTextBox().SelectionStart + form.getTextBox().SelectedText.Length, form.getTextArea().Length));
                    form.getTextBox().SelectionStart = cp;
                    return;
                }
                if (form.getTextBox().SelectionStart == form.getTextArea().Length)
                {
                    form.setTextArea(form.getTextArea().Substring(0, form.getTextBox().SelectionStart));
                    form.getTextBox().SelectionStart = cp;
                    return;
                }
            }
        }
        public void paste(SimpleForm form)
        {
            String clipboardText = Clipboard.GetText();
            
            if (form.getTextBox().SelectionStart == 0)
            {
                form.setTextArea(clipboardText + form.getTextArea());
                return;
            }
            if (form.getTextBox().SelectionStart > 0 && form.getTextBox().SelectionStart < form.getTextArea().Length)
            {
                form.setTextArea(form.getTextArea().Substring(0, form.getTextBox().SelectionStart) + clipboardText + form.getTextArea().Substring(form.getTextBox().SelectionStart, form.getTextArea().Length));
                return;
            }
            if (form.getTextBox().SelectionStart == form.getTextArea().Length)
            {
                form.setTextArea(form.getTextArea() + clipboardText);
                return;
            }
        }
        public void delete(SimpleForm form)
        {
            if (form.getTextBox().SelectedText.Length != null)
            {
                form.setAction(false);
                form.setTextArea(form.getTextArea().Substring(0, form.getTextBox().SelectionStart) + form.getTextArea().Substring(form.getTextBox().SelectionLength, form.getTextArea().Length));
                form.getList().newPutString(form.getTextBox().SelectedText, form.getTextBox().SelectionStart, actionsHistory.action.SUB);
                form.setAction(true);
            }
        }
        public void backspace(SimpleForm form)
        {
            if (form.getTextBox().SelectedText.Length != null)
            {
                form.setAction(false);
                form.setTextArea(form.getTextArea().Substring(0, form.getTextBox().SelectionStart) + form.getTextArea().Substring(form.getTextBox().SelectionLength, form.getTextArea().Length));
                form.getList().newPutString(form.getTextBox().SelectedText, form.getTextBox().SelectionStart, actionsHistory.action.SUB);
                form.setAction(true);
            }
        }

    }
}

