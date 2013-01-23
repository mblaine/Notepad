using System;
using System.Collections.Generic;
using System.Windows.Forms;

public class StatefulTextBox : TextBox
{
    protected class State
    {
        public String Text = null;
        public int SelectionStart = 0;

        public State(String text, int selectionStart)
        {
            this.Text = text;
            this.SelectionStart = selectionStart;
        }

        public override string ToString()
        {
            return this.Text;
        }
    }

    protected Stack<State> undo;
    protected Stack<State> redo;
    protected State current;

    protected volatile bool suppressEvents = false;
    protected volatile bool caretMoveNeedsRecorded = false;

    public StatefulTextBox()
        : base()
    {
        current = new State("", 0);
        undo = new Stack<State>();
        redo = new Stack<State>();
        undo.Push(current);
    }

    public new void Clear()
    {
        base.Clear();
        current = new State("", 0);
        undo = new Stack<State>();
        redo = new Stack<State>();
        undo.Push(current);
    }

    protected void RecordState()
    {
        redo.Clear();
        if(undo.Count == 0 || undo.Peek().Text != current.Text)
            undo.Push(current);
        current = new State(this.Text, this.SelectionStart);
    }

    public new bool CanUndo
    {
        get
		{
            return undo.Count > 0;
		}
    }

    public bool CanRedo
    {
        get
		{
            return redo.Count > 0;
		}
    }

    public new void Undo()
    {
        if (undo.Count > 0)
        {
            if (redo.Count == 0 || current.Text != redo.Peek().Text)
                redo.Push(current);
            current = undo.Pop();
            suppressEvents = true;
            this.Text = current.Text;
            this.SelectionStart = current.SelectionStart;
            suppressEvents = false;
        }
    }

    public void Redo()
    {
        if (redo.Count > 0)
        {
            if (undo.Count == 0 || current.Text != undo.Peek().Text)
                undo.Push(current);
            current = redo.Pop();
            suppressEvents = true;
            this.Text = current.Text;
            this.SelectionStart = current.SelectionStart;
            suppressEvents = false;
        }
    }

    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);
        if (caretMoveNeedsRecorded)
        {
            caretMoveNeedsRecorded = false;
            RecordState();
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        switch(e.KeyCode)
        {
            case Keys.Delete:
            case Keys.Back:
            case Keys.Enter:
                RecordState();
                break;

            case Keys.Up:
            case Keys.Down:
            case Keys.Left:
            case Keys.Right:
            case Keys.PageUp:
            case Keys.PageDown:
            case Keys.Home:
            case Keys.End:
                if (caretMoveNeedsRecorded)
                {
                    caretMoveNeedsRecorded = false;
                    RecordState();
                }
                break;
        }
        base.OnKeyDown(e);
    }

    protected override void OnTextChanged(EventArgs e)
    {
        if (!suppressEvents)
        {
            caretMoveNeedsRecorded = true;
            current = new State(this.Text, this.SelectionStart);
        }
        base.OnTextChanged(e);
    }
}