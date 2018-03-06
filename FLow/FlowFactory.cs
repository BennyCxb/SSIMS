using BT.Manage.Core;
using BT.Manage.Frame.Base;
using FLow.FLows;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLow
{
    /// <summary>
    /// 流程工厂类
    /// </summary>
    public class FlowFactory
    {
        public static IFlow GetFlowClass(int flowEnum)
        {
            IFlow flowClass = null;
            switch (flowEnum)
            {
                case (int)FlowEnum.省级问题:
                    flowClass=new _1000011Flow();
                    break;
                case (int)FlowEnum.市级问题:
                    flowClass = new _1000012Flow();
                    break;
                case (int)FlowEnum.县级问题:
                    flowClass = new _1000013Flow();
                    break;
                default: break;
            }

            return flowClass;
        }

        public static BaseModel GetModel(int flowEnum)
        {
            BaseModel model = null;
            switch(flowEnum)
            {
                case (int)FlowEnum.省级问题:
                    model = null;
                    break;
                case (int)FlowEnum.市级问题:
                    model = null;
                    break;
                case (int)FlowEnum.县级问题:
                    model = null;
                    break;
                default: break;
            }

            return model;
        }
    }

   
}
