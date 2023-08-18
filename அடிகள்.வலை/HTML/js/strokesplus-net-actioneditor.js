const ModifierCapture_Before = 0;
const ModifierCapture_After = 1;
const ModifierCapture_Either = 2;

const ActionType_ActionEngine = 0;
const ActionType_ScriptEngine = 1;


/********************************************************************************************
    ActionEditor
*********************************************************************************************/

class ActionEditor {

    constructor(id, actionList) {
        this.Id = id;
        this.ActionList = actionList;
        this.ActionItem = null;
        this.StepsScriptEditor = new StepsScriptEditor(this.Id, `#Details_${this.Id},#DetailsHeader_${this.Id}`);
        this.NameChangeTimer; 
    }

    get HTML() {
        return `<div id="DetailsHeader_${this.Id}" class="form-row pb-3">
                    <div class="col-auto mr-2">
                        <label class="control-label" for="StrokeButton_${this.Id}">${stringEscapeHtml(`||tabPreferencesStrokeButtonGroupText||`)}</label>
                        <select class="form-control" id="StrokeButton_${this.Id}">
                            <option value="Primary" ${this.ActionItem.Action.UseSecondaryStrokeButton ? "" : "selected"}>${stringEscapeHtml(`||Primary||`)}</option>
                            <option value="Secondary" ${this.ActionItem.Action.UseSecondaryStrokeButton ? "selected" : ""}>${stringEscapeHtml(`||Secondary||`)}</option>
                        </select>
                    </div>
                    <div class="col-auto mr-2">
                        <label class="control-label" for="Category_${this.Id}">${stringEscapeHtml(`||Category||`)}</label>
                        <select class="form-control actionEditorCategory" id="Category_${this.Id}">
                            ${this.CategoryOptionsHTML}
                        </select>
                    </div>
                    <div class="col-auto flex-fill mr-4">
                        <label class="control-label" for="Name_${this.Id}">${stringEscapeHtml(`||Name||`)}</label>
                        <input type="text" class="form-control actionEditorName" id="Name_${this.Id}" placeholder="${stringEscape(`||Name||`)}" value="${stringEscapeProperty(this.ActionItem.Action.Description)}">
                    </div>
                    <div class="float-right mr-4 mt-4 ml-2">
                            <div class="form-check pr-4 mt-1 d-block">
                                <input type="checkbox" class="form-check-input form-check-large" id="Active_${this.Id}" ${this.ActionItem.Action.Active ? "checked" : ""}>
                                <label class="form-check-label form-check-large-label" for="Active_${this.Id}">${stringEscapeHtml(`||ActionActiveLabel||`)}</label>
                            </div>
                    </div>
                </div>
                <div id="Details_${this.Id}" class="form-row actionEditorDetails">
                    <div class="col-auto border mr-2 mb-2">
                        <label class="pr-2 control-label">${stringEscapeHtml(`||Gesture||`)}</label>
                        <canvas class="d-block pt-1 px-2 cursor-pointer" id="canvas_${this.Id}" data-usesecondary="${this.ActionItem.Action.UseSecondaryStrokeButton}" width="125" height="125"></canvas>
                        <a class="pl-2" href="#" onclick="$(this).parent().find('canvas').click();">
                            <label class="cursor-pointer text-center actionEditorGestureLabel" id="GestureName_${this.Id}">${this.ActionItem.Action.GestureName}</label>
                        </a>
                    </div>
                    <div class="col-auto border mr-2 mb-2 actionEditorRegionContainer ${this.ActionList.Application.RegionType === RegionType_None ? "d-none" : ""}">
                        <label class="pr-2 control-label">${stringEscapeHtml(`||RegionLabel||`)}</label>
                        <div class="d-block pt-1 px-2" id="regions_${this.Id}">
                            ${this.GetRegionsHTML()}
                        </div>
                    </div>
                    <div id="ModifierPanel_${this.Id}" class="col-auto border mr-2 mb-2">
                        <label class="pr-2 control-label">${stringEscapeHtml(`||Modifiers||`)}</label>
                        <div class="pb-2 pl-2 form-row">
                            <div class="col">
                                <div class="form-check pr-2 pb-1 d-block text-nowrap">
                                    <input type="checkbox" class="form-check-input" id="ModifierControl_${this.Id}" ${this.ActionItem.Action.Control ? "checked" : ""}>
                                    <label class="form-check-label" for="ModifierControl_${this.Id}">${stringEscapeHtml(`||ControlKey||`)}</label>
                                </div>
                                <div class="form-check pr-2 pb-1 d-block text-nowrap">
                                    <input type="checkbox" class="form-check-input" id="ModifierAlt_${this.Id}" ${this.ActionItem.Action.Alt ? "checked" : ""}>
                                    <label class="form-check-label" for="ModifierAlt_${this.Id}">${stringEscapeHtml(`||AltKey||`)}</label>
                                </div>
                                <div class="form-check pr-2 pb-1 d-block text-nowrap">
                                    <input type="checkbox" class="form-check-input" id="ModifierShift_${this.Id}" ${this.ActionItem.Action.Shift ? "checked" : ""}>
                                    <label class="form-check-label" for="ModifierShift_${this.Id}">${stringEscapeHtml(`||ShiftKey||`)}</label>
                                </div>
                                <div class="form-check pr-2 pb-1 d-block text-nowrap">
                                    <input type="checkbox" class="form-check-input" id="ModifierWheelUp_${this.Id}" ${this.ActionItem.Action.WheelUp ? "checked" : ""}>
                                    <label class="form-check-label" for="ModifierWheelUp_${this.Id}">${stringEscapeHtml(`||WheelUp||`)}</label>
                                </div>
                                <div class="form-check pr-2 pb-1 d-block text-nowrap">
                                    <input type="checkbox" class="form-check-input" id="ModifierWheelDown_${this.Id}" ${this.ActionItem.Action.WheelDown ? "checked" : ""}>
                                    <label class="form-check-label" for="ModifierWheelDown_${this.Id}">${stringEscapeHtml(`||WheelDown||`)}</label>
                                </div>
                            </div>
                            <div class="col">
                                <div class="form-check pr-2 pb-1 d-block text-nowrap">
                                    <input type="checkbox" class="form-check-input" id="ModifierLeft_${this.Id}" ${this.ActionItem.Action.Left ? "checked" : ""}>
                                    <label class="form-check-label" for="ModifierLeft_${this.Id}">${stringEscapeHtml(`||LeftMouse||`)}</label>
                                </div>
                                <div class="form-check pr-2 pb-1 d-block text-nowrap">
                                    <input type="checkbox" class="form-check-input" id="ModifierMiddle_${this.Id}" ${this.ActionItem.Action.Middle ? "checked" : ""}>
                                    <label class="form-check-label" for="ModifierMiddle_${this.Id}">${stringEscapeHtml(`||MiddleMouse||`)}</label>
                                </div>
                                <div class="form-check pr-2 pb-1 d-block text-nowrap">
                                    <input type="checkbox" class="form-check-input" id="ModifierRight_${this.Id}" ${this.ActionItem.Action.Right ? "checked" : ""}>
                                    <label class="form-check-label" for="ModifierRight_${this.Id}">${stringEscapeHtml(`||RightMouse||`)}</label>
                                </div>
                                <div class="form-check pr-2 pb-1 d-block text-nowrap">
                                    <input type="checkbox" class="form-check-input" id="ModifierX1_${this.Id}" ${this.ActionItem.Action.X1 ? "checked" : ""}>
                                    <label class="form-check-label" for="ModifierX1_${this.Id}">${stringEscapeHtml(`||tabPreferencesStrokeButtonX1||`)}</label>
                                </div>
                                <div class="form-check pr-2 pb-1 d-block text-nowrap">
                                    <input type="checkbox" class="form-check-input" id="ModifierX2_${this.Id}" ${this.ActionItem.Action.X2 ? "checked" : ""}>
                                    <label class="form-check-label" for="ModifierX2_${this.Id}">${stringEscapeHtml(`||tabPreferencesStrokeButtonX2||`)}</label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="ModifierCapturePanel_${this.Id}" class="col-auto pr-3 border mr-2 mb-2 d-none">
                        <label class="pr-2 control-label">${stringEscapeHtml(`||ModifierCaptureLabel||`)}</label>
                        <div class="large-radio pl-2 pb-1 text-nowrap">
                            <input type="radio" id="CaptureEither_${this.Id}" name="Capture_${this.Id}" value="Either" ${this.ActionItem.Action.Capture === ModifierCapture_Either ? "checked" : ""}>
                            <label class="form-check-label" for="CaptureEither_${this.Id}">${stringEscapeHtml(`||ModifierCaptureEither||`)}</label>
                        </div>
                        <div class="large-radio pl-2 pb-1 text-nowrap">
                            <input type="radio" id="CaptureBefore_${this.Id}" name="Capture_${this.Id}" value="Before" ${this.ActionItem.Action.Capture === ModifierCapture_Before ? "checked" : ""}>
                            <label class="form-check-label" for="CaptureBefore_${this.Id}">${stringEscapeHtml(`||ModifierCaptureBefore||`)}</label>
                        </div>
                        <div class="large-radio pl-2 pb-1 text-nowrap">
                            <input type="radio" id="CaptureAfter_${this.Id}" name="Capture_${this.Id}" value="After" ${this.ActionItem.Action.Capture === ModifierCapture_After ? "checked" : ""}>
                            <label class="form-check-label" for="CaptureAfter_${this.Id}">${stringEscapeHtml(`||ModifierCaptureAfter||`)}</label>
                        </div>
                    </div>
                    <div id="HintTextPanel_${this.Id}" class="col-auto border mr-4 mb-2">
                        <div class="w-100 h-100">
                            <label class="pr-2 control-label">${stringEscapeHtml(`||HintText||`)}</label>
                            <textarea id="HintText_${this.Id}" placeholder="${stringEscapeHtml(`||EnterHintText||`)}" class="form-control w-100 resize-none actionEditorHintText">${this.ActionItem.Action.Comments ?? ""}</textarea>
                        </div>
                    </div>                    
                </div>
                ${this.StepsScriptEditor.HTML}
                `;
    }

   
    get CategoryOptionsHTML() {
        let catOptionsHTML = "";
        let categories = this.ActionList.Categories;
        for(let i = 0; i < categories.length; i++) {
            catOptionsHTML += `<option value="${stringEscapeProperty(categories[i])}" ${categories[i] == this.ActionItem.Action.Category ? "selected" : ""}>${stringEscapeHtml(categories[i])}</option>`;
        }
        return catOptionsHTML;
    }
    
    DrawGesture() {
        gestures.DrawToCanvas(appSettings.Gestures.find(g => g.Name == this.ActionItem.Action.GestureName), 
            `canvas_${this.Id}`,
            null,
            colorNetToHex(this.ActionItem.Action.UseSecondaryStrokeButton ? appSettings.SecondaryPenColor : appSettings.PenColor)
        ); 
    }
    
    GetRegionsHTML() {
        let regions = this.ActionItem.Action.RegionColRows;
        let regionHTML = "";
    
        if(regions && this.ActionList.Application.RegionType !== RegionType_None) {
    
            //Determine grid
            let cols = 0;
            let rows = 0;
    
            switch(this.ActionList.Application.RegionType) {
                case RegionType_VerticalSplit:
                    cols = 2;
                    rows = 1;
                    break;
                case RegionType_HorizontalSplit:
                    cols = 1;
                    rows = 2;
                    break;
                case RegionType_Quadrant:
                    cols = 2;
                    rows = 2;
                    break;
                case RegionType_Grid:
                    cols = 3;
                    rows = 3;
                    break;                                                
                case RegionType_Custom:
                    cols = this.ActionList.Application.RegionCustomCols;
                    rows = this.ActionList.Application.RegionCustomRows;
                    break;
            }
    
            if(cols > 0 && rows > 0) {
                let cellWidth = 125 / cols;
                let cellHeight = 125 / rows;
    
                regionHTML = `<table class="border">
                                <tbody>`;
                for(let r = 0; r < rows; r++) {
                    regionHTML += '<tr>';
                    for(let c = 0; c < cols; c++) {
                        let selected = '';
                        for(let ri = 0; ri < regions.length; ri++){
                            if(regions[ri].RegionColumn - 1 === c && regions[ri].RegionRow - 1 === r) {
                                selected = 'checked';
                            }
                        }
                        regionHTML += `<td class="border text-center" style="width:${cellWidth}px;height:${cellHeight}px;">
                                            <input class="regionCheck" id="regionCheck_${this.Id}_R${r + 1}_C${c + 1}" ${selected} type="checkbox">
                                       </td>`;
                    }
                    regionHTML += '</tr>';
                }
                regionHTML += ` </tbody>
                              </table>`;
    
            }
        }
        return regionHTML;
    }
    
    LoadAction(actionItem) {
        if(this.ActionItem) {
            this.StepsScriptEditor.Save();
            this.Save(true);
        }
    
        this.ActionItem = actionItem;
        this.StepsScriptEditor.LoadAction(this, this.ActionItem.Action);
    
        $(`#${this.Id}`).html(this.HTML);
        this.PostRender();
    }
    
    NameChange(actionEditor) {
        clearTimeout(this.NameChangeTimer);
        this.NameChangeTimer = setTimeout( function(actionEditor) {
                                               clearTimeout(this.NameChangeTimer);
                                               actionEditor.Save();
                                           }, 750, actionEditor);
    }
    
    PostRender() {
        this.DrawGesture();
        this.ShowRegionPanel();
        this.ShowModifierCapture();   
        this.StepsScriptEditor.PostRender();
    
        document.getElementById(`StrokeButton_${this.Id}`).addEventListener("change", () => this.Save(false))
        document.getElementById(`Category_${this.Id}`).addEventListener("change", () => this.Save(false))
        $(`#Name_${this.Id}`).on("input propertychange paste", () => this.NameChange(this));
        $(`#Details_${this.Id} input`).on("change", () => this.Save(false));
        $(`#Active_${this.Id}`).on("change", () => this.Save(false))
        $(`#canvas_${this.Id}`).on("click", () => this.ShowGestureSelect());
    }
    
    Save(noConflictCheck) {
        if(this.ActionItem) {
    
            this.ActionItem.Action.Active = $(`#Active_${this.Id}`).prop('checked');
    
            this.ActionItem.Action.Control = $(`#ModifierControl_${this.Id}`).prop('checked');
            this.ActionItem.Action.Alt = $(`#ModifierAlt_${this.Id}`).prop('checked');
            this.ActionItem.Action.Shift = $(`#ModifierShift_${this.Id}`).prop('checked');
            this.ActionItem.Action.WheelUp = $(`#ModifierWheelUp_${this.Id}`).prop('checked');
            this.ActionItem.Action.WheelDown = $(`#ModifierWheelDown_${this.Id}`).prop('checked');
            this.ActionItem.Action.Left = $(`#ModifierLeft_${this.Id}`).prop('checked');
            this.ActionItem.Action.Middle = $(`#ModifierMiddle_${this.Id}`).prop('checked');
            this.ActionItem.Action.Right = $(`#ModifierRight_${this.Id}`).prop('checked');
            this.ActionItem.Action.X1 = $(`#ModifierX1_${this.Id}`).prop('checked');
            this.ActionItem.Action.X2 = $(`#ModifierX2_${this.Id}`).prop('checked');        
    
            switch($(`[name=Capture_${this.Id}]:checked`).val()) {
                case "Either":
                    this.ActionItem.Action.Capture = ModifierCapture_Either;
                    break;
                case "Before":
                    this.ActionItem.Action.Capture = ModifierCapture_Before;
                    break;
                case "After":
                    this.ActionItem.Action.Capture = ModifierCapture_After;
                    break;
            }
    
            this.ActionItem.Action.Category = $(`#Category_${this.Id}`).val();
            this.ActionItem.Action.Description = $(`#Name_${this.Id}`).val();
            this.ActionItem.Action.Comments = $(`#HintText_${this.Id}`).val();

            this.ActionItem.Action.GestureName = $(`#GestureName_${this.Id}`).text();
    
            this.ActionItem.Action.UseSecondaryStrokeButton = $(`#StrokeButton_${this.Id}`).val() === "Secondary";
    
            $(`#canvas_${this.Id}`).data('usesecondary', this.ActionItem.Action.UseSecondaryStrokeButton);
    
            if(this.ActionList.Application.RegionType !== RegionType_None) {
                let regions = [];
                let checkIdPrefix = `regionCheck_${this.Id}_`;
                $(`[id^=regionCheck_${this.Id}]`).each(function(index, item) {
                    if($(item).prop('checked')) {
                        let col = 0;
                        let row = 0;
                        let vals = $(item).prop('id').replace(checkIdPrefix,'').split('_');
                        row = vals[0].replace('R','');
                        col = vals[1].replace('C','');
                        regions.push({"RegionColumn":col,"RegionRow":row});
                    }
                });
                this.ActionItem.Action.RegionColRows = regions;
            }
    
            this.StepsScriptEditor.Save();

            this.DrawGesture();
            this.ShowRegionPanel();
            this.ShowModifierCapture();           
    
            this.ActionItem.Update(noConflictCheck);
        }
    }
    
    SetHeight(height) {
        //log('ActionEditor.AdjustHeight');
        $(`#${this.Id}`).height(height);
        if(this.StepsScriptEditor) {
            this.StepsScriptEditor.Resize();
        }
    }

    ShowGestureSelect() {
        gestures.ShowSelectModal();
    }
    
    ShowModifierCapture() {
        if(this.ActionItem.Action.Control || this.ActionItem.Action.Alt || this.ActionItem.Action.Shift || this.ActionItem.Action.WheelUp 
            || this.ActionItem.Action.WheelDown || this.ActionItem.Action.Left || this.ActionItem.Action.Middle || this.ActionItem.Action.Right 
            || this.ActionItem.Action.X1 || this.ActionItem.Action.X2) {
            $(`#ModifierCapturePanel_${this.Id}`).removeClass('d-none');
        } else {
            $(`#ModifierCapturePanel_${this.Id}`).addClass('d-none');        
        }   
    }
    
    ShowRegionPanel() {
        if(this.ActionList.Application.RegionType !== RegionType_None) {
            $(`${this.Id} .actionEditorRegionContainer`).removeClass('d-none');
        } else {
            $(`${this.Id} .actionEditorRegionContainer`).addClass('d-none');
        }
    }
    
    UpdateGesture(gestureName) {
        modalDismissStandardCenter();
        this.ActionItem.Action.GestureName = gestureName;
        $(`#GestureName_${this.Id}`).text(gestureName);
        this.Save();
    }    
}


