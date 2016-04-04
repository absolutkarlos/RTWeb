﻿using System;

namespace GoldDataWeb.Models
{
	public class StatusBarViewModel
	{
		public Data Nuevo { get; set; }
		public Data Prefactibility { get; set; }
		public Data Inspection { get; set; }
		public Data Instalation { get; set; }
	}

	public class Data
	{
		public string EstimatedDateNextStep { get; set; }
		public string Class { get; set; }
		public string Color { get; set; } = @"none";
	}
}
