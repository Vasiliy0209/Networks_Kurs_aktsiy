using System;
using System.Windows.Forms;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;


namespace downloadJSONCurrencies
{

	public partial class Form1 : Form
	{
		//Адрес веб-сайта, содержащего данные о курсах акций
		string baseURL = "https://quote.rbc.ru/v5/ajax/catalog/get-tickers?type=share&sort=blue_chips&limit=200&offset=0";
		HttpClient client;

		public Form1()
		{
			InitializeComponent();
			//Настройка параметров сетевого соединения
			this.client = new HttpClient();
			this.client.MaxResponseContentBufferSize = 2560000;
		}

		//Определение метода, загружающего курс акций с веб-сайта
		public async void getData()
		{
			//Очистка таблицы на форме
			this.dataGridView1.Rows.Clear();
			
			//Скачивание акций с веб-сайта в формате JSON в переменную data
			HttpResponseMessage response = await client.GetAsync(this.baseURL);
			response.EnsureSuccessStatusCode();

			string data = await response.Content.ReadAsStringAsync();
			
			//Преобразование JSON-данных из переменной data в массив объектов языка C#
			var parsedData = JsonSerializer.Deserialize<List<Currency>>(data);

			//Вывод объектов из полученного массива в таблицу на форме
			foreach(var item in parsedData)
			{
				this.dataGridView1.Rows.Add(item.company.title, item.price, item.currency);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			//Запуск скачивания акций
			this.getData();
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}
	}
	
	//Описание объектов языка C#, в которые будут извлечены данные
	//полученные с веб-сайта с курсами акций
	public class Company
	{
		public string title { get; set; }
	}
	
	public class Currency
	{
		public string title { get; set; }
		public double price { get; set; }
		public string currency { get; set; }
		public Company company { get; set; }
	}
}
