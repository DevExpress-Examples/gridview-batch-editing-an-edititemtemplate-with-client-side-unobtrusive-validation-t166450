﻿// Developer Express Code Central Example:
// GridView - Batch Editing - A simple implementation of an EditItem template
// 
// This example demonstrates how to create a custom editor inside column's DataItem
// template when GridView is in Batch Edit mode.
// 
// 
// You can implement the
// EditItem template for a column by performing the following steps:
// 
// 1. Specify
// column's EditItem template:    settings.Columns.Add(column =>    {
// column.FieldName = "C1";      column.SetEditItemTemplateContent(c =>      {
// @Html.DevExpress().SpinEdit(spinSettings =>        {
// spinSettings.Name = "C1spinEdit";          spinSettings.Width =
// System.Web.UI.WebControls.Unit.Percentage(100);
// spinSettings.Properties.ClientSideEvents.KeyDown = "C1spinEdit_KeyDown";
// spinSettings.Properties.ClientSideEvents.LostFocus = "C1spinEdit_LostFocus";
// }).Render();      });    });
// 
// 
// 
// 2. Handle grid's client-side
// BatchEditStartEditing
// (https://help.devexpress.com/#AspNet/DevExpressWebASPxGridViewScriptsASPxClientGridView_BatchEditStartEditingtopic)
// event to set the grid's cell values to the editor. It is possible to get the
// focused cell value using the e.rowValues
// (https://help.devexpress.com/#AspNet/DevExpressWebASPxGridViewScriptsASPxClientGridViewBatchEditStartEditingEventArgs_rowValuestopic)
// property:    function Grid_BatchEditStartEditing(s, e) {      var
// productNameColumn = s.GetColumnByField("C1");      if
// (!e.rowValues.hasOwnProperty(productNameColumn.index))        return;      var
// cellInfo = e.rowValues[productNameColumn.index];
// C1spinEdit.SetValue(cellInfo.value);      if (e.focusedColumn ===
// productNameColumn)        C1spinEdit.SetFocus();    }
// 
// 
// 
// 3. Handle the
// BatchEditEndEditing
// (https://help.devexpress.com/#AspNet/DevExpressWebASPxGridViewScriptsASPxClientGridView_BatchEditEndEditingtopic)
// event to pass the value entered in the editor to the grid's cell:    function
// Grid_BatchEditEndEditing(s, e) {      var productNameColumn =
// s.GetColumnByField("C1");      if
// (!e.rowValues.hasOwnProperty(productNameColumn.index))        return;      var
// cellInfo = e.rowValues[productNameColumn.index];      cellInfo.value =
// C1spinEdit.GetValue();      cellInfo.text = C1spinEdit.GetText();
// C1spinEdit.SetValue(null);    } 4. The BatchEditRowValidating
// (https://help.devexpress.com/#AspNet/DevExpressWebASPxGridViewScriptsASPxClientGridView_BatchEditRowValidatingtopic)
// event allows validating the grid's cell based on the entered value:    function
// Grid_BatchEditRowValidating(s, e) {      var productNameColumn =
// s.GetColumnByField("C1");      var cellValidationInfo =
// e.validationInfo[productNameColumn.index];      if (!cellValidationInfo) return;
// var value = cellValidationInfo.value;      if
// (!ASPxClientUtils.IsExists(value) || ASPxClientUtils.Trim(value) === "") {
// cellValidationInfo.isValid = false;        cellValidationInfo.errorText = "C1
// is required";      }    } 5. Finally, handle the editor's client-side KeyDown
// (https://documentation.devexpress.com/AspNet/DevExpressWebASPxEditorsScriptsASPxClientTextEdit_KeyDowntopic.aspx)
// and LostFocus
// (https://documentation.devexpress.com/AspNet/DevExpressWebASPxEditorsScriptsASPxClientEdit_LostFocustopic.aspx)
// events to emulate the behavior of standard grid editors when an end-user uses a
// keyboard or mouse:    var preventEndEditOnLostFocus = false;    function
// C1spinEdit_KeyDown(s, e) {      var keyCode =
// ASPxClientUtils.GetKeyCode(e.htmlEvent);      if (keyCode !== ASPxKey.Tab &&
// keyCode !== ASPxKey.Enter) return;      var moveActionName =
// e.htmlEvent.shiftKey ? "MoveFocusBackward" : "MoveFocusForward";      if
// (grid.batchEditApi[moveActionName]()) {
// ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
// preventEndEditOnLostFocus = true;      }    }    function
// C1spinEdit_LostFocus(s, e) {      if (!preventEndEditOnLostFocus)
// grid.batchEditApi.EndEdit();      preventEndEditOnLostFocus = false;    }  See
// Also:
// http://www.devexpress.com/scid=T115096
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=T115130

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GridViewBatchEdit {
	public class RouteConfig {
		public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");

            routes.MapRoute(
                name: "Default", // Route name
                url: "{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }
	}
}