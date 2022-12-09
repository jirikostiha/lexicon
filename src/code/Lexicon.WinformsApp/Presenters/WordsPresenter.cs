using Lexicon;
using Lexicon.EntityModel;
using Lexicon.WinformsApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WordsPresenter
{
    private readonly IWordsView _view;

    private readonly WordMultiSourceProvider _multiSourceProvider;
    
    public WordsPresenter(IWordsView view, WordMultiSourceProvider multiSourceProvider)
    {
        _view = view;
        view.Presenter = this;
        _multiSourceProvider = multiSourceProvider;
        
        UpdateListView();
    }

    private void UpdateListView()
    {
        //var customerNames = from customer in _repository.GetAllCustomers() select customer.Name;
        //int selectedCustomer = _view.SelectedCustomer >= 0 ? _view.SelectedCustomer : 0;
        //_view.CustomerList = customerNames.ToList();
        //_view.SelectedCustomer = selectedCustomer;
    }

    //public void UpdateView(int index)
    //{
    //    WordRecord wordRecord = _multiSourceProvider.Get(p);
    //    //_view.CustomerName = customer.Name;
    //    //_view.Address = customer.Address;
    //    //_view.Phone = customer.Phone;
    //}
}