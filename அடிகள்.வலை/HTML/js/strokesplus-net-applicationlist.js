class ApplicationList {
    constructor(applications) {
        this.Applications = applications;
        this.SelectedApplication;
        if(appSettings.LastApplication.length > 0) {
            this.SelectedApplication = this.Applications.find(a => a.Description === appSettings.LastApplication);
        } else if(this.Applications.length > 0) {
            this.SelectedApplication = this.GetApplications()[0];
        }
        this.ActionList = new ActionList('tabApplicationsActionList', this.SelectedApplication, 50, 50);
        this.Load();
    }

    get CurrentApplication() {
        log('ApplicationList:CurrentApplication');
        return this.SelectedApplication;
    }

    get CurrentActionItem() {
        log('ApplicationList:CurrentActionItem');
        return this.ActionList.CurrentAction;
    }

    get HTML() {
        return `<div>
                    <!-- Tabs -->
                    <ul class="nav nav-tabs nav-justified">
                        <li class="nav-item" title="${stringEscapeProperty(`||SettingsToolbarApplications||`)}">
                            <a class="nav-link active" data-toggle="tab" href="#tabApplicationsActionList">${stringEscapeHtml(`||SettingsToolbarApplications||`)}</a>
                        </li>
                        <li class="nav-item" title="${stringEscapeProperty(`||TextExpansion||`)}">
                            <a class="nav-link" data-toggle="tab" href="#tabApplicationsTextExpansion">${stringEscapeHtml(`||TextExpansion||`)}</a>
                        </li>
                        <li class="nav-item" title="${stringEscapeProperty(`||frmAppBeforeAfterTab||`)}">
                            <a class="nav-link" data-toggle="tab" href="#tabApplicationsBeforeAfter">${stringEscapeHtml(`||frmAppBeforeAfterTab||`)}</a>
                        </li>
                        <li class="nav-item" title="${stringEscapeProperty(`||frmAppDefinitionTab||`)}">
                            <a class="nav-link" data-toggle="tab" href="#tabApplicationsAppDefinition">${stringEscapeHtml(`||frmAppDefinitionTab||`)}</a>
                        </li>
                    </ul>

                    <!-- Tab panes -->
                    <div class="tab-content border-top">
                        <div class="tab-pane active" id="tabApplicationsActionList">
                            ${this.ActionList.HTML}
                        </div>
                        <div class="tab-pane" id="tabApplicationsTextExpansion">
                            TEXT EXPANSIONS
                        </div>
                        <div class="tab-pane" id="tabApplicationsBeforeAfter">
                            BEFORE/AFTER
                        </div>
                        <div class="tab-pane" id="tabApplicationsAppDefinition">
                            App Definition
                        </div>
                    </div>
                </div>`;        
    }

    GetApplications() {
        return [...new Set(this.Applications.map(item => item.Description))].sort((a, b) => a.localeCompare(b, undefined, { sensitivity: 'accent' }));
    }

    Load() {
        log('ApplicationList:Load');
    }

    PostRender() {
        log('ApplicationList:PostRender');
        this.ActionList.PostRender();
    }
}