﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KOT.DataModel.Model;

namespace KOT.DataModel.Handlers
{
    public class BudgetHandler
    {
        private static BudgetHandler _instance;
        private SpendingModel _model;

        public static BudgetHandler Instance
        {
            get { return _instance ?? (_instance = new BudgetHandler()); }
        }

        public BudgetHandler()
        {
            _instance = this;
        }

        public static SpendingModel Model { get { return Instance._model; } }

        public static async Task UpdateSource()
        {
            await Instance.UpdateSourceAsync();
        }
        private DateTime _current = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private async Task UpdateSourceAsync()
        {
            //if (DesignMode.DesignModeEnabled)
            {
                _model = new SpendingModel { DID = "1", TotalCost = 6456 };
                _model.Spends.Add(new OneSpendItem
                {
                    Date = new DateDataModel(_current.AddDays(-35)),
                    Name = "Comment 1",
                    Sum = 156,
                    idClass = 0
                });
                _model.Spends.Add(new OneSpendItem
                {
                    Date = new DateDataModel(_current.AddDays(-34)),
                    Name = "Comment 2",
                    Sum = 268,
                    idClass = 1
                });
                _model.Spends.Add(new OneSpendItem
                {
                    Date = new DateDataModel(_current.AddDays(-35)),
                    Name = "Comment 3",
                    Sum = 878,
                    idClass = 2
                });
                _model.Spends.Add(new OneSpendItem
                {
                    Date = new DateDataModel(_current.AddDays(-5)),
                    Name = "Comment 4",
                    Sum = 658,
                    idClass = 3
                });
                _model.Spends.Add(new OneSpendItem
                {
                    Date = new DateDataModel(_current.AddDays(-1)),
                    Name = "Comment 5",
                    Sum = 984,
                    idClass = 4
                });
                _model.Spends.Add(new OneSpendItem
                {
                    Date = new DateDataModel(_current.AddDays(-65)),
                    Name = "Comment 6",
                    Sum = 741,
                    idClass = 2
                });
            }
        }
    }
}