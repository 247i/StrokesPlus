/********************************************************************************************
    StepsScriptEditor
*********************************************************************************************/


class StepsScriptEditor {

    constructor(parentId, toggleTarget) {
        this.Id = `stepsScriptEditor_${parentId}_${Date.now()}`;
        this.ToggleTarget = toggleTarget;
        this.ActionEditor = null;
        this.Action = null;
        //this.StepsList = new StepsList();
        this.ScriptEditor = null;
    }

    get HTML() {
        return `<div id="ToggleDetails_${this.Id}" class="d-flex align-items-center mr-3 ToggleScriptEditorParent">
                    <div class="btn-group btn-group-toggle" >
                        <label id="ActionTypeSteps_${this.Id}" class="btn btn-secondary ${this.Action.Type === ActionType_ActionEngine ? "active" : ""} btn-sm">
                            ${stringEscapeHtml(`||ActionTab||`)}
                        </label>
                        <label id="ActionTypeScript_${this.Id}" class="btn btn-secondary ${this.Action.Type === ActionType_ScriptEngine ? "active" : ""} btn-sm">
                            ${stringEscapeHtml(`||ScriptTab||`)}
                        </label>
                    </div>       
                    <div>
                        <div class="actionStepsScriptToolbar">${this.ToolbarHTML}</div>
                    </div> 
                    <a id="ToggleIcon_${this.Id}" href="#" class="ml-auto">
                        <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-chevrons-up ${!this.ToggleTarget ? "d-none" : ""}" width="32" height="32" viewBox="0 0 24 24" stroke-width="1.5" stroke="#9e9e9e" fill="none" stroke-linecap="round" stroke-linejoin="round">
                            <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                            <polyline points="7 11 12 6 17 11" />
                            <polyline points="7 17 12 12 17 17" />
                        </svg>
                        <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-chevrons-down d-none" width="32" height="32" viewBox="0 0 24 24" stroke-width="1.5" stroke="#9e9e9e" fill="none" stroke-linecap="round" stroke-linejoin="round">
                            <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                            <polyline points="7 7 12 12 17 7" />
                            <polyline points="7 13 12 18 17 13" />
                        </svg>
                    </a>
                </div>
                <div id="Steps_${this.Id}" class="${this.Action.Type === ActionType_ActionEngine ? "" : "ffd-none"}">
                    <p>Steps</p>
                </div>
                <div id="Script_${this.Id}" class="${this.Action.Type === ActionType_ScriptEngine ? "" : "ffd-none"}">
                    <textarea id="script_${this.Id}"></textarea>
                </div>
                `;
    }    

    LoadAction(actionEditor, action) {
        this.ActionEditor = actionEditor;
        this.Action = action;
        log('StepsScriptEditor.LoadAction()');
    }

    get MenuHtml() {
        return ``;
    }

    OnToggle() {
        if($(this.ToggleTarget).hasClass('d-none')) {
            $(this.ToggleTarget).removeClass('d-none');
            $(`#ToggleDetails_${this.Id}`).find('.icon-tabler-chevrons-down').addClass('d-none');
            $(`#ToggleDetails_${this.Id}`).find('.icon-tabler-chevrons-up').removeClass('d-none');
            appSettings.SettingsActionEditorDetailsCollapsed = false;
        } else {
            $(this.ToggleTarget).addClass('d-none');
            $(`#ToggleDetails_${this.Id}`).find('.icon-tabler-chevrons-up').addClass('d-none');
            $(`#ToggleDetails_${this.Id}`).find('.icon-tabler-chevrons-down').removeClass('d-none');            
            appSettings.SettingsActionEditorDetailsCollapsed = true;   
        }
        this.ScriptEditor.Resize();
    }

    OnTypeChange(e) {
        e.preventDefault();
        if(e.currentTarget.id === `ActionTypeScript_${this.Id}`) {
            $(`#ActionTypeSteps_${this.Id}`).removeClass('active');
            $(`#Steps_${this.Id}`).addClass('d-none');
            $(`#ActionTypeScript_${this.Id}`).addClass('active');
            $(`#Script_${this.Id}`).removeClass('d-none');
        } else {
            $(`#ActionTypeScript_${this.Id}`).removeClass('active');
            $(`#Script_${this.Id}`).addClass('d-none');
            $(`#ActionTypeSteps_${this.Id}`).addClass('active');
            $(`#Steps_${this.Id}`).removeClass('d-none');
        }
        this.Save();
    }

    OnToolbarMenuItemClick(action) {
        switch(action) {
            case 'scriptRecord':
                window.currentSection.ActionList.ActionEditor.StepsScriptEditor.ScriptEditor.Record();
                break;
            default:
                log('StepsScript:OnToolbarMenuItemClick - unknown - action: ' + action);
        }        
    }

    PostRender() {
        this.ScriptEditor = new ScriptEditor(this.ActionEditor.Id, `script_${this.Id}`, this.Action);

        if(this.ToggleTarget && appSettings.SettingsActionEditorDetailsCollapsed) {
            this.OnToggle();
        }

        if(this.Action.Type === ActionType_ActionEngine) {
            $(`#Script_${this.Id}`).addClass('d-none');
        } else {
            $(`#Steps_${this.Id}`).addClass('d-none');
        }

        document.getElementById(`ToggleIcon_${this.Id}`).addEventListener("click", () => this.OnToggle());
        document.getElementById(`ActionTypeSteps_${this.Id}`).addEventListener("click", (e) => this.OnTypeChange(e));
        document.getElementById(`ActionTypeScript_${this.Id}`).addEventListener("click", (e) => this.OnTypeChange(e));

        $(`#ToggleDetails_${this.Id} .actionStepsScriptToolbar .toolbarItem`).on('click', function(e){
            window.currentSection.ActionList.ActionEditor.StepsScriptEditor.OnToolbarMenuItemClick($(e.currentTarget).data('action'));
        });        
    }

    Resize() {
        if(this.ScriptEditor) {
            this.ScriptEditor.Resize();
        }
    }

    Save() {
        if($(`#ActionTypeSteps_${this.Id}`).hasClass('active')) {
            this.Action.Type = ActionType_ActionEngine;
        } else {
            this.Action.Type = ActionType_ScriptEngine;
        }

        //this.StepsList.Save();
        this.ScriptEditor.Save();
        log('StepsScriptEditor.Save()');
    }    

    get ToolbarHTML() {

        return `<span class="toolbarItem pr-1" 
                      data-action="scriptRecord" 
                      title="${stringEscapeProperty(`||Record||`)}">
                    <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-folder-plus" 
                        width="24" height="24" viewBox="0 0 24 24" stroke-width="1.5" fill="none" stroke-linecap="round" stroke-linejoin="round">
                        <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                        <path d="M5 4h4l3 3h7a2 2 0 0 1 2 2v8a2 2 0 0 1 -2 2h-14a2 2 0 0 1 -2 -2v-11a2 2 0 0 1 2 -2" />
                        <line x1="12" y1="10" x2="12" y2="16" />
                        <line x1="9" y1="13" x2="15" y2="13" />
                    </svg>
                    <span>${stringEscapeHtml(`||Record||`)}</span>
                </span>`;    

    }

}