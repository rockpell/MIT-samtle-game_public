using UnityEngine;
using System;
using System.Collections;

[AttributeUsage ( AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true )]
public class HideAttribute : PropertyAttribute
{
	public string _sourceField = "";
	public int _enumIndex;
	public bool _hideInInspector = false;

	public HideAttribute ( string sourceField )
	{
		_sourceField = sourceField;
		_hideInInspector = false;
	}

	public HideAttribute ( string conditionalSourceField, EventType type )
	{
		int flag = (int) type;
		int index = 0;

		while ( flag != 0 )
		{
			flag /= 2;
			index++;
		}

		_sourceField = conditionalSourceField;
		_enumIndex = index;
		_hideInInspector = true;
	}

	public HideAttribute ( string conditionalSourceField, Condition type )
	{
		_sourceField = conditionalSourceField;
		_enumIndex = (int) type;
		_hideInInspector = true;
	}

	public HideAttribute ( string conditionalSourceField, InteractFunction type )
	{
		_sourceField = conditionalSourceField;
		_enumIndex = (int)type;
		_hideInInspector = true;
	}
}
