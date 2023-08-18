/********************************************************************************************
    ActionList
*********************************************************************************************/

class ActionList {

    constructor(container, application, gestureWidth, gestureHeight) {

        this.Id = `actionlist_${application.Description}_${Date.now()}`.idname();
        this.Container = container;
        this.Application = application;
        this.ActionItems = [];
        this.GestureWidth = gestureWidth || 125;
        this.GestureHeight = gestureHeight || 125;
        this.ActionEditor = new ActionEditor(`actioneditor_${this.Id}`, this);
        this.CurrentActionItem;
        this.Load();

    }

    get HTML() {

        return `<div class="container-fluid">
                    <div class="row flex-nowrap">
                        <div id="${this.Id}" class="d-block">
                            <div id="accordion_${this.Id}" class="actionListAccordion">
                                <div class="actionListToolbar">${this.ToolbarHTML}</div>
                                <div class="actionListCardContainer">                            
                                    ${this.AccordionHTML}
                                </div>
                            </div>
                        </div>
                        <div id="${this.ActionEditor.Id}" class="pl-3 d-block flex-fill actionEditor"></div>
                    </div>
                </div>`;

    }

    AddAction() {

        log('AddAction()');

    }

    AddCategory() {

        log('AddCategory()');

    }

    AdjustHeight() {

        navSetTabsWidth();
        log('footer top: ' + $('.footer').position().top);
        log('actionlist top: ' + $(`#${this.Id}`).position().top);
        let height = $('.footer').position().top - $(`#${this.Id}`).position().top;
        $(`#accordion_${this.Id}`).height(height);
        this.ActionEditor.SetHeight(height);
        let actionListWidth = parseInt($(`#accordion_${this.Id}`).width());
        appSettings.SettingsActionListWidth = actionListWidth < 150 ? 300 : actionListWidth;
        log('ActionList.AdjustHeight');

    }

    CheckActionConflicts(noAlert) {

        let actionItemList = { 
            Application: this.Application,
            Id: this.Id,
            ActionItems: []
        };
        this.ActionItems.forEach(function(val, index) {
            actionItemList.ActionItems.push({ Action: val.Action, Id: val.Id });
        });
        hostPostMessage('CheckActionConflicts', noAlert, actionItemList);    

    }

    Copy() {

        log('Copy()');

    }

    Cut() {

        log('Cut()');

    }

    Delete() {

        log('Delete()');

    }

    Export() {

        log('Export()');

    }

    get AccordionHTML() {

        let accordionHTML = "";
        var uniqueSortedCategories = this.Categories; 
    
        for(let c = 0; c < uniqueSortedCategories.length; c++) {
            accordionHTML += this.GetCategoryHTML(uniqueSortedCategories[c], this.ActionItems.filter(a => a.Action.Category === uniqueSortedCategories[c])) + '\n';
        }
    
        return accordionHTML;

    }

    get Categories() {

        return [...new Set(this.Application.Actions.map(item => item.Category))].sort((a, b) => a.localeCompare(b, undefined, { sensitivity: 'accent' }));

    }

    GetCategory(category) {

        return $(`#collapsecategory_${category.idname()}_${this.Id}`);

    }

    GetCategoryHTML(category, actionItems) {

        return `<div class="card">
                    <div class="card-header" 
                         id="category_${category.idname()}_${this.Id}" 
                         data-toggle="collapse" 
                         data-target="#collapsecategory_${category.idname()}_${this.Id}" 
                         aria-expanded="${((window.currentSection instanceof SectionGlobalActions && appSettings.LastGlobalActionCategory === category) || (!(window.currentSection instanceof SectionGlobalActions) && appSettings.LastApplicationCategory == category))}">
                        <h5 class="mb-0">
                            ${stringEscapeHtml(category)}
                        </h5>
                    </div>
    
                    <div id="collapsecategory_${category.idname()}_${this.Id}" class="collapse" aria-labelledby="category_${category.idname()}_${this.Id}" data-parent="#${this.Id}">
                        ${this.GetCategoryItemsHTML(actionItems)}
                    </div>
                </div>`;
    
    }

    GetCategoryItemsHTML(actionItems) {
    
        let actionsHTML = "";
        let sortedActions = actionItems.sort((a, b) => a.Action.Description.localeCompare(b.Action.Description, undefined, { sensitivity: 'accent' }))
        for(let i = 0; i < sortedActions.length; i++) {
            actionsHTML += sortedActions[i].HTML + "\n";
        }
        return actionsHTML;
    
    }    

    get MenuHtml() {
        return ``;
    }

    Import() {

        log('Import()');

    }

    Load() {

        for(let i = 0; i < this.Application.Actions.length; i++){
            this.ActionItems.push(new ActionListItem(this, this.Application.Actions[i]));
        }

    } 
    
    PostRender() {

        this.ActionItems.forEach(function(item, index) {
            item.PostRender();
        });
     
        $(`#accordion_${this.Id}`).width(appSettings.SettingsActionListWidth < 150 ? 300 : appSettings.SettingsActionListWidth);
    
        if(window.currentSection instanceof SectionGlobalActions) {
            this.SelectAction(this.ActionItems.find(a => a.Action.Category === appSettings.LastGlobalActionCategory && a.Action.Description === appSettings.LastGlobalActionName), true);
        } else {
            this.SelectAction(this.ActionItems.find(a => a.Action.Category === appSettings.LastApplicationCategory && a.Action.Description === appSettings.LastApplicationActionName), true);
        }
       
        new ResizeObserver(settingsResizeElements).observe(document.getElementById(this.Id));

        $(`#accordion_${this.Id} .actionListToolbar .toolbarItem`).on('click', function(e){
            window.currentSection.ActionList.OnToolbarMenuItemClick($(e.currentTarget).data('action'));
        });

        this.AdjustHeight();
        this.CheckActionConflicts(false);

    }

    Rename() {

        log('Rename()');

    }

    SaveLastSelection(actionItem) {

        if(window.currentSection instanceof SectionGlobalActions) {
            appSettings.LastGlobalActionCategory = actionItem.Action.Category;
            appSettings.LastGlobalActionName = actionItem.Action.Description;
        } else {
            appSettings.LastApplicationCategory = actionItem.Action.Category;
            appSettings.LastApplicationActionName = actionItem.Action.Description;
        }

    }

    SelectAction(actionItem, scrollIntoView) {

        if(this.CurrentAction !== actionItem) {
            $(`#${this.Id} .card-body`).removeClass('active');
            $(`#${actionItem.Id}`).addClass('active');
            $(`#${actionItem.Id}`).closest('.collapse').collapse('show');
            this.SaveLastSelection(actionItem);
            if(scrollIntoView) {
                setTimeout(function() {
                                        $(`#${actionItem.Id}`)[0].scrollIntoView()
                                    }, 0);
            }
            this.ActionEditor.LoadAction(actionItem);
            this.CurrentAction = actionItem;
            this.CurrentAction.CheckForConflict();
        }

    }    

    ToggleActive() {

        log('ToggleActive()');

    }

    get ToolbarHTML() {

        return `<span class="toolbarItem" 
                      data-action="addCategory" 
                      title="${stringEscapeProperty(`||AddCategory||`)}">
                    <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-folder-plus" 
                        width="24" height="24" viewBox="0 0 24 24" stroke-width="1.5" fill="none" stroke-linecap="round" stroke-linejoin="round">
                        <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                        <path d="M5 4h4l3 3h7a2 2 0 0 1 2 2v8a2 2 0 0 1 -2 2h-14a2 2 0 0 1 -2 -2v-11a2 2 0 0 1 2 -2" />
                        <line x1="12" y1="10" x2="12" y2="16" />
                        <line x1="9" y1="13" x2="15" y2="13" />
                    </svg>
                </span>

                <span class="toolbarItem" 
                      data-action="addAction" 
                      title="${stringEscapeProperty(`||AddAction||`)}">
                    <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-plus" 
                        width="24" height="24" viewBox="0 0 24 24" stroke-width="1.5" fill="none" stroke-linecap="round" stroke-linejoin="round">
                        <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                        <line x1="13" y1="5" x2="13" y2="19" />
                        <line x1="6" y1="12" x2="20" y2="12" />
                    </svg>
                </span>

                <span class="toolbarItem" 
                      data-action="toggleActive" 
                      title="${stringEscapeProperty(`||ToggleActive||`)}">
                    <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-edit" 
                        width="24" height="24" viewBox="0 0 24 24" stroke-width="1.5" fill="none" stroke-linecap="round" stroke-linejoin="round">
                        <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                        <path d="M9 7h-3a2 2 0 0 0 -2 2v9a2 2 0 0 0 2 2h9a2 2 0 0 0 2 -2v-3" />
                        <path d="M9 15h3l8.5 -8.5a1.5 1.5 0 0 0 -3 -3l-8.5 8.5v3" />
                        <line x1="16" y1="5" x2="19" y2="8" />
                    </svg>
                </span>`;    

    }

    OnToolbarMenuItemClick(action) {
        switch(action) {
            case 'addCategory':
                window.currentSection.ActionList.AddCategory();
                break;
            case 'addAction':
                window.currentSection.ActionList.AddAction();
                break;
            case 'copy':
                window.currentSection.ActionList.Copy();
                break;      
            case 'cut':
                window.currentSection.ActionList.Cut();
                break;   
            case 'delete':
                window.currentSection.ActionList.Delete();
                break;               
            case 'export':
                window.currentSection.ActionList.Export();
                break;
            case 'import':
                window.currentSection.ActionList.Import();
                break;
            case 'rename':
                window.currentSection.ActionList.Rename();
                break;                         
            case 'toggleActive':
                window.currentSection.ActionList.ToggleActive();
                break;
        }        
    }
}


/********************************************************************************************
    ActionListItem
*********************************************************************************************/

class ActionListItem {

    constructor(actionList, action) {

        this.Action = action;
        this.ActionList = actionList;
        this.Id = `actionlistitem_${this.ActionList.Application.Description}_${this.Action.Category}_${this.Action.Description}_${Date.now()}`.idname();

    }

    get Canvas() {

        return $(`#canvas_${this.Id}`);

    }

    get HTML() {

        return `<div class="card-body actionlistitem" id="${this.Id}" data-category="${stringEscapeProperty(this.Action.Category)}" data-description="${stringEscapeProperty(this.Action.Description)}">
                    <div class="media">
                        <div class="media-left align-self-center">
                            <canvas id="canvas_${this.Id}" 
                                    class="mt-1" 
                                    title="${stringEscapeProperty(this.Action.Description)}"
                                    width="${this.ActionList.GestureWidth}" 
                                    height="${this.ActionList.GestureHeight}" 
                                    data-gesturename="${stringEscapeProperty(this.Action.GestureName)}" 
                                    data-regions="${this.ActionList.Application.RegionType !== RegionType_None && this.Action.RegionColRows.length > 0 
                                                     ? JSON.stringify(this.Action.RegionColRows).replace(/"/g, "'")
                                                     : ''}" 
                                    data-usesecondary="${this.Action.UseSecondaryStrokeButton}" 
                                    data-active="${this.Action.Active}">
                            </canvas>
                        </div>
                        <div class="media-body align-self-center pl-3">
                            <h6 class="mb-0 ${!this.Action.Active ? "text-muted" : ""}">${stringEscapeHtml(this.Action.Description)}</h6>
                            <small>${this.BuildSmallText()}</small>
                        </div>
                    </div>
                </div>`;

    }    

    BuildSmallText() {

        let smallText = "";
    
        if(colorNetToHex(appSettings.SecondaryPenColor) === colorNetToHex(appSettings.PenColor)) {
            if(this.Action.UseSecondaryStrokeButton) {
                smallText = "<em>||Secondary||</em><br />";    
            } else {
                smallText = "<em>||Primary||</em><br />";    
            }
        }
    
        let modifiers = "";
        if(this.Action.Control) {
            modifiers += "||ControlKey||, ";
        }
        if(this.Action.Alt) {
            modifiers += "||AltKey||, ";
        }
        if(this.Action.Shift) {
            modifiers += "||ShiftKey||, ";
        }
        if(this.Action.WheelUp) {
            modifiers += "||WheelUp||, ";
        }
        if(this.Action.WheelDown) {
            modifiers += "||WheelDown||, ";
        }    
        if(this.Action.Left) {
            modifiers += "||LeftMouse||, ";
        }
        if(this.Action.Middle) {
            modifiers += "||MiddleMouse||, ";
        }
        if(this.Action.Right) {
            modifiers += "||RightMouse||, ";
        }
        if(this.Action.X1) {
            modifiers += "||X1Mouse||, ";
        } 
        if(this.Action.X2) {
            modifiers += "||X2Mouse||, ";
        }                 
        if(modifiers.length > 0) {
            modifiers = modifiers.slice(0,-2);
        }
        smallText += modifiers;                 
        if(modifiers.length > 0 && this.Action.Capture !== ModifierCapture_Either) {
            switch(this.Action.Capture) {
                case ModifierCapture_Before:
                    smallText += " <em>(||ModifierCaptureBefore||)</em>";
                    break;
                case ModifierCapture_After:
                    smallText += " <em>(||ModifierCaptureAfter||)</em>";
                    break;                            
            }
        }  
    
        return smallText;

    }

    CheckForConflict() {

        alertClearFooter('conflictmessage');
        hostPostMessage('ActionExists', '', { "Application" : this.ActionList.Application, "Action" : this.Action, "Target" : this.Id });

    }

    DrawGesture() {

        gestures.DrawToCanvas(appSettings.Gestures.find(g => g.Name == this.Action.GestureName), 
            $(`#canvas_${this.Id}`).prop('id'),
            null,
            colorNetToHex(this.Action.UseSecondaryStrokeButton ? appSettings.SecondaryPenColor : appSettings.PenColor),
            this.Action.RegionColRows
        );        

    }

    OnClick(event) {

        this.ActionList.SelectAction(this, false);

    }

    PostRender() {

        this.DrawGesture();   
        document.getElementById(`${this.Id}`).addEventListener("click", () => this.OnClick());

    }

    Update(noConflictCheck) {

        $(`#${this.Id}`).closest('.card-body').find('h6').text(this.Action.Description);
        this.Canvas.prop('title', stringEscapeProperty(this.Action.Description));
    
        this.UpdateSmallText();
    
        if($(`#${this.Id}`).data('category') !== this.Action.Category || $(`#${this.Id}`).data('description') !== this.Action.Description){
            let i = 1;
            let description = this.Action.Description;
            while(this.ActionList.ActionItems.find(a => a.Category === this.Action.Category 
                                                                 && a.Description === this.Action.Description
                                                                 && a.Id !== this.Id)) {
                this.Action.Description = description + ' ' + i;
                i++;
            }
    
            $(`#${this.Id}`).data('category', this.Action.Category);
            $(`#${this.Id}`).data('description', this.Action.Description);
            $(`#${this.Id}`).closest('.card-body').find('h6').text(this.Action.Description);
        
            let itemMoved = false;
            let targetCategory = this.ActionList.GetCategory(this.Action.Category);        
            let sourceId = this.Id;
            $(targetCategory).children().each(function(index, item){
                let currentItem = $(item);
                if($(`#${sourceId}`).data('description').localeCompare(currentItem.data('description'), undefined, { sensitivity: 'accent' }) < 0) {
                    $(`#${sourceId}`).insertBefore($(currentItem));
                    itemMoved = true;
                    return false;
                }
            });
            if(!itemMoved) {
                $(targetCategory).append($(`#${this.Id}`));
                itemMoved = true;
            }
            $(targetCategory).collapse('show');
        
            setTimeout(function(element) {
                $(element)[0].scrollIntoView();
                $(element).removeClass('graybgfade');
                $(element).addClass('graybgfade');
            }, 0, `#${this.Id}`);
        }
    
        $(`#canvas_${this.Id}`).data('gesturename', this.Action.GestureName);
        $(`#canvas_${this.Id}`).data('regions', this.ActionList.Application.RegionType !== RegionType_None && this.Action.RegionColRows.length > 0 
                                                ? JSON.stringify(this.Action.RegionColRows).replace(/"/g, "'")
                                                : '');
        $(`#canvas_${this.Id}`).data('usesecondary', this.Action.UseSecondaryStrokeButton);
        $(`#canvas_${this.Id}`).data('active', this.Action.Active);
    
        if(this.Action.Active) {
            $(`#${this.Id}`).closest('.card-body').find('h6').removeClass('text-muted');
        } else {
            $(`#${this.Id}`).closest('.card-body').find('h6').addClass('text-muted');
        }   
    
        if(noConflictCheck) {
            $(`#${this.Id}`).closest('.card-body').find('h6').removeClass('text-danger');
        } else {
            this.CheckForConflict();
        }
    
        this.DrawGesture();      
    
        this.ActionList.CheckActionConflicts(true);

    }

    UpdateSmallText() {

        $(`#${this.Id}`).closest('.card-body').find('small').html(this.BuildSmallText());

    }  
}
