using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ParceQuery
/// </summary>
public class ParceQuery
{
    private IParceQuery _iparceQuery; 
	public ParceQuery(IParceQuery parceQuery)
	{
	    _iparceQuery = parceQuery;
	}

   

}