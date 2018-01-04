using System;
using System.Collections.Generic;
using System.Linq;

namespace Outils.TConsole
{
    /// <summary>
    /// Modélise un menu à afficher dans la console
    /// </summary>
    public class Menu
	{
		private IDictionary<string, Option> _options;

        /// <summary>
        /// Dictionnaire des entrée de menu (en lecture seule)
        /// </summary>
        public IReadOnlyDictionary<string, Option> Options
        {
            get { return new System.Collections.ObjectModel.ReadOnlyDictionary<string, Option>(_options); }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        public Menu()
		{
			_options = new Dictionary<string, Option>();
		}

		/// <summary>
		/// Affiche le menu, puis lance l'action correspondant à l'entrée choisie par l'utilisateur
		/// </summary>
		public void Prompt()
		{
			string choice = string.Empty;
			do
			{
				// Affichage du menu
				foreach (var kvp in _options)
					Console.WriteLine(kvp.Value.ToString());

				// Lecture du choix
				do
					choice = Input.Read<string>("Choix :");
				while (!_options.Keys.Contains(choice));

				// Exécution du traitement associé au menu
				Console.WriteLine();
				_options[choice].Callback();
			}
			while (choice != "s" && choice != "a" && choice != "p");
		}

		/// <summary>
		/// Crée une entrée de menu et l'ajoute au dictionnaire
		/// </summary>
		/// <param name="id">Identifiant de l'entrée de menu</param>
		/// <param name="label">Libellé de l'entrée de menu</param>
		/// <param name="callback">Traitement associé à l'entrée de menu</param>
		/// <returns>Le menu complet</returns>
		public Menu AddOption(string id, string label, Action callback)
		{
			return AddOption(new Option(id, label, callback));
		}

		/// <summary>
		/// Ajoute une entrée de menu existante au dictionnaire
		/// </summary>
		/// <param name="opt">Identifiant de l'entrée de menu</param>
		/// <returns>Le menu complet</returns>
		public Menu AddOption(Option opt)
		{
			Option op;
			if (_options.TryGetValue(opt.Id, out op))
				throw new InvalidOperationException("Une entrée de menu avec cet Id existe déjà !");

			_options.Add(opt.Id, opt);
			return this;
		}
	}

	/// <summary>
	/// Entrée de menu
	/// </summary>
	public class Option
	{
		/// <summary>
		/// Identifiant utilisé pour sélectionner l'entrée de menu
		/// </summary>
		public string Id { get; }

		/// <summary>
		/// Libellé de l'entrée de menu
		/// </summary>
		public string Label { get; }

		/// <summary>
		/// Traitement associé à l'entrée de menu
		/// </summary>
		public Action Callback { get; }

		/// <summary>
		/// Crée une entrée de menu
		/// </summary>
		/// <param name="label">Nom de l'entrée de menu</param>
		/// <param name="callback">Traitement à exécuter</param>
		public Option(string id, string label, Action callback)
		{
			Id = id;
			Label = label;
			Callback = callback;
		}

		public override string ToString()
		{
			return string.Format("{0}. {1}", Id, Label);
		}
	}
}
