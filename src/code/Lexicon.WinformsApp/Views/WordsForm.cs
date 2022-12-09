using Lexicon.EntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Lexicon.WinformsApp.Views
{
    public partial class WordsForm : Form, IWordsView
    {
        private WordsPresenter _presenter;

        public WordsForm()
        {
            InitializeComponent();
            _presenter = new WordsPresenter(this);
        }

        public IList<WordRecord> WordList 
        { 
            get => (IList<WordRecord>)WordListBox.DataSource;
            set => WordListBox.DataSource = value;
        }

        public int SelectedItem
        { 
            get => WordListBox.SelectedIndex; 
            set => WordListBox.SelectedIndex = value; 
        }
        //public WordMetadata Metadata 
        //{ 
        //    get => throw new NotImplementedException(); 
        //    set => throw new NotImplementedException(); 
        //}
    }
}
