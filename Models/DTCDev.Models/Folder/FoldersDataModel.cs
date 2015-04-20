using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Folder
{

    /// <summary>
    /// Класс для возврата данных о папках
    /// </summary>
    public class FoldersDataModel
    {
        /// <summary>
        /// Название папки
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Уникальный идентификатор папки
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Айди папки. которая является родительской для текущей папки
        /// </summary>
        public int previosID { get; set; }



        public List<int> IncludedCars { get; set; }
    }
}
