using Magikarp.Platform.Behavior;
using Magikarp.Platform.Definition;
using Magikarp.Platform.Definition.Pakage;
using Magikarp.Platform.UI.Entry;
using Magikarp.Utility.TransitData;
using System;
using System.Collections.Generic;

namespace Magikarp.Application.Asurada
{
    static class Entry
    {
        [STAThread]
        static void Main()
        {
            List<System.Reflection.Assembly> objCustomAssemblys = new List<System.Reflection.Assembly>();
            MainViewDTO objDTO = new MainViewDTO();
           

            int nStep = 0;//程序進度指標。
            Boolean bRun = true;//程序中斷旗標。

            while (bRun)
            {
                nStep += 1;
                switch (nStep)
                {   
                    case 1: // 載入自訂函式庫。
                        AssemblyManager.GetInstance().MountAssembly(objCustomAssemblys);
                        break;

                    case 2: // 設定功能入口資訊。
                        foreach (System.Reflection.Assembly objAssembly in objCustomAssemblys)
                        {
                            object[] objResult = objAssembly.GetCustomAttributes(typeof(FunctionEntryAttribute), false);

                            if (objResult != null)
                            {
                                foreach (object objAttribute in objResult)
                                {
                                    objDTO.AddFunctioinEntryModel(((FunctionEntryAttribute)objAttribute).EntryModel);
                                }
                            }
                        }
                        break;

                    case 3:// 載入共用操作畫面。
                        AssemblyManager.GetInstance().MountAssembly(typeof(MainViewModel).Assembly);
                        break;

                    case 4:// 載入平台核心。
                        AssemblyManager.GetInstance().MountAssembly(typeof(FlowController).Assembly);
                        break;

                    case 5: // 透過核心啟動系統。                          
                        ((IController)new FlowController()).DispatchCommand("Show_Main", new TransitDataAdapter().Export<MainViewDTO>(objDTO));
                        break;

                    default://結束。
                        bRun = false;
                        break;
                }
            }
        }
    }
}
