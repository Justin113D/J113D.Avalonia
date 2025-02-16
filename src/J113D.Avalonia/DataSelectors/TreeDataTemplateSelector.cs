using Avalonia.Controls.Templates;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;
using System;
using System.Collections.Generic;

namespace J113D.Avalonia.DataSelectors
{
	/// <summary>
	/// Template selector for tree data
	/// </summary>
	public class TreeDataTemplateSelector : ITreeDataTemplate
	{
		/// <summary>
		/// Templates available for being selected
		/// </summary>
		[Content]
		public List<TreeDataTemplate> AvailableTemplates { get; } = [];

		private TreeDataTemplate? GetTemplate(object data)
		{
			Type type = data.GetType();

			foreach(TreeDataTemplate template in AvailableTemplates)
			{
				if(type.IsAssignableTo(template.DataType))
				{
					return template;
				}
			}

			return null;
		}

		/// <inheritdoc/>
		public InstancedBinding? ItemsSelector(object item)
		{
			return GetTemplate(item)?.ItemsSelector(item);
		}

		/// <inheritdoc/>
		public bool Match(object? data)
		{
			return true;
		}

		Control? ITemplate<object?, Control?>.Build(object? param)
		{
			ArgumentNullException.ThrowIfNull(param);
			return GetTemplate(param)?.Build(param) ?? throw new ArgumentException("Could not find appropriate template!", nameof(param));
		}
	}
}
