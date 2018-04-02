using BLL.Common;
using FluentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TZManageAPI.DTO;

namespace BLL
{
    public class MenuBll :BaseBll
    {
        #region 返回导航条Json
        public static List<Menu> ReturnJson(int userId)
        {
            return GetMenuList(userId);
        }
        #endregion

        #region 根据角色获得一级菜单
        public static List<Menu> GetMenuList(int userId)
        {
            List<Menu> list = DbContext.Sql(@"select distinct m.FID,
m.FName
,m.FParentID,
m.FUrl as FUrlPath,
m.FBillTypeID,
m.FParameters,
m.FSort
 from 
t_sys_MenusForRole mr 
left join t_sys_Menu m on m.FID=mr.FMenuID and isnull(m.FIsDeleted,0)=0
where mr.FRoleID in (
	select ru.FRoleID from t_sys_Users u
		left join t_sys_RolesForUser ru on ru.FUserID=u.FID
		where u.FID=@userId and m.FID is not null 
) 
order by m.FSort asc ").Parameter("userId", userId).QueryMany<Menu>();

            List<Menu> rootList = new List<Menu>();
            foreach (var _item in list.Where(p => p.FParentID == 999))
            {
                Menu _node = new Menu();
                _node.FID = _item.FID;
                _node.FName = _item.FName;
                _node.FParentID = _item.FParentID;
                _node.FUrlPath = _item.FUrlPath;
                _node.FBillTypeID = _item.FBillTypeID;
                _node.FParameters = _item.FParameters;
                _node.FChild = GetChildMenuList(list, _node);
                rootList.Add(_node);
            }
            return rootList;
        }
        #endregion

        #region 递归查询子级

        private static List<Menu> GetChildMenuList(List<Menu> list, Menu item)
        {
            int parentMenuID = item.FID;
            List<Menu> nodeList = new List<Menu>();
            var children = list.Where(t => t.FParentID == parentMenuID);
            foreach (var _item in children)
            {
                Menu _node = new Menu();
                _node.FID = _item.FID;
                _node.FName = _item.FName;
                _node.FParentID = _item.FParentID;
                _node.FUrlPath = _item.FUrlPath;
                _node.FBillTypeID = _item.FBillTypeID;
                _node.FParameters = _item.FParameters;
                _node.FChild = GetChildMenuList(list, _node);
                nodeList.Add(_node);
            }
            return nodeList;
        }
        #endregion

    }
}
