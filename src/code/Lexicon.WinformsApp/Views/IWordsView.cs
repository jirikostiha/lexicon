using Lexicon.EntityModel;
using Lexicon.WinformsApp;
using System;
using System.Collections.Generic;

//https://www.codeproject.com/Articles/22761/Model-View-Presenter-Using-Dependency-Injection-an
//https://github.com/theilgaz/winforms-dependency-injection/tree/main/src/winforms-dependency-injection
//https://github.com/mrts/winforms-mvp/blob/master/WinFormsMVP/WinFormsMVP/Presenter/CustomerPresenter.cs

public interface IWordsView
{
    IList<WordRecord> WordList { get; set; }

    int SelectedItem { get; set; }

    //WordMetadata Metadata { get; set; }

    WordsPresenter Presenter { set; }
}
