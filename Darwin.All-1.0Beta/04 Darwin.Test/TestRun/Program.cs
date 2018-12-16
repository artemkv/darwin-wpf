using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Server;

namespace TestRun
{
	class Program
	{
		static void Main(string[] args)
		{
			AssemblerTest asm = new AssemblerTest();
			asm.GetDTO_SimpleProp_Test();
//			asm.GetRealObject_Author_Test();
//			asm.GetRealObject_AuthorBook_Insert_Test();
//			asm.GetRealObject_AuthorBook_Update_Test();
//			asm.GetRealObject_AuthorBook_UpdateAndAttach_Test();
//			asm.GetRealObject_AuthorBook_Delete_Test();
		}
	}
}
