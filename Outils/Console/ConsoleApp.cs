using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Outils.TConsole
{
	/// <summary>
	/// Modélise une application console avec des pages et une navigation entre les pages
	/// </summary>
	public abstract class ConsoleApplication
	{
		// Dictinnaire des pages de l'application
		// Chaque page est identifiée par son type
		private Dictionary<Type, Page> _pages;

		#region Propriétés publiques

		/// <summary>
		/// Obtient ou définit le titre affiché dans la barre de titre de la console 
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Obtient la page courante (la dernière de l'historique)
		/// </summary>
		public Page CurrentPage
		{
			get
			{
				return (History.Any()) ? History.Peek() : null;
			}

			private set
			{
				// Si la page n'est pas déjà la page courante
				if (!History.Any() || History.Peek().GetType() != value.GetType())
				{
					// On récupère la page et on la remet en haut de la pile
					Page newPage;
					if (_pages.TryGetValue(value.GetType(), out newPage))
						History.Push(newPage);
					else
						throw new KeyNotFoundException("Il n'existe pas de page " + value.GetType().ToString() + " dans l'application");
				}
			}
		}

		/// <summary>
		/// Obtient ou définit si un en-tête de navigation doit être affiché
		/// </summary>
		public bool NavigationHeader { get; set; }

		/// <summary>
		/// Obtient l'historique des pages affichées
		/// </summary>
		public Stack<Page> History { get; private set; }

		/// <summary>
		/// Indique si la navigation est activée
		/// </summary>
		public bool NavigationEnabled { get { return History.Count > 1; } }
		#endregion

		// Constructeur
		public ConsoleApplication()
		{
			_pages = new Dictionary<Type, Page>();
			History = new Stack<Page>();
			NavigationHeader = true;
		}

		#region Méthodes publiques
		/// <summary>
		/// Lance l'application en affichant la page de démarrage
		/// </summary>
		public virtual void Run()
		{
			try
			{
				Console.Title = Title;
				DisplayPage(CurrentPage);
			}
			catch (Exception e)
			{
				Output.WriteLine(ConsoleColor.Red, e.ToString());
			}
			finally
			{
				if (Debugger.IsAttached)
				{
					Input.Read<string>("Press [Enter] to exit");
				}
			}
		}

		/// <summary>
		/// Ajoute la page passée en paramètre à la liste des pages de l'appli
		/// Si une page de même type existe déjà, elle est remplacée par la nouvelle		/// </summary>
		/// <param name="page"></param>
		public void AddPage(Page page)
		{
			Type pageType = page.GetType();

			if (_pages.ContainsKey(pageType))
				_pages[pageType] = page;
			else
				_pages.Add(pageType, page);
		}

		/// <summary>
		/// Retourne à la page de démarrage
		/// </summary>
		public void NavigateHome()
		{
			while (History.Count > 1)
				History.Pop();

			Console.Clear();
			DisplayPage(CurrentPage);
		}

		/// <summary>
		/// Navigue vers une page donnée
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public Page NavigateTo(Page page)
		{
			CurrentPage = page;

			Console.Clear();
			DisplayPage(CurrentPage);
			return CurrentPage;
		}

		public Page NavigateTo(Type type)
		{
			Page p;
			if (_pages.TryGetValue(type, out p))
				NavigateTo(p);
			else
				throw new KeyNotFoundException("Il n'existe pas de page " + type.ToString() + " dans l'application");

			return p;
		}

		/// <summary>
		/// Retourne à la page précédente
		/// </summary>
		/// <returns></returns>
		public Page NavigateBack()
		{
			History.Pop();

			Console.Clear();
			DisplayPage(CurrentPage);
			return CurrentPage;
		}
		#endregion

		private void DisplayPage(Page page)
		{
			// Affiche l'en-tête de navigation ou le titre de l'appli
			if (History.Count > 1 && NavigationHeader)
			{
				string header = null;
				foreach (var title in History.Select((p) => p.Title).Reverse())
					header += title + " > ";
				header = header.Remove(header.Length - 3);
				Console.WriteLine(header);
			}
			else
			{
				Console.WriteLine(Title);
			}
			Console.WriteLine("---");

			// Affiche la page, avec son menu éventuel
			page.Display();
		}
	}
}
