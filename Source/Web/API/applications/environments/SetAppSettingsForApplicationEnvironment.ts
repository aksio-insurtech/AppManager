/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/app-settings');

export interface ISetAppSettingsForApplicationEnvironment {
    applicationId?: string;
    environmentId?: string;
    content?: string;
}

export class SetAppSettingsForApplicationEnvironmentValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        content: new Validator(),
    };
}

export class SetAppSettingsForApplicationEnvironment extends Command<ISetAppSettingsForApplicationEnvironment> implements ISetAppSettingsForApplicationEnvironment {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/app-settings';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetAppSettingsForApplicationEnvironmentValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _content!: string;

    constructor() {
        super(Object, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'environmentId',
            'content',
        ];
    }

    get applicationId(): string {
        return this._applicationId;
    }

    set applicationId(value: string) {
        this._applicationId = value;
        this.propertyChanged('applicationId');
    }
    get environmentId(): string {
        return this._environmentId;
    }

    set environmentId(value: string) {
        this._environmentId = value;
        this.propertyChanged('environmentId');
    }
    get content(): string {
        return this._content;
    }

    set content(value: string) {
        this._content = value;
        this.propertyChanged('content');
    }

    static use(initialValues?: ISetAppSettingsForApplicationEnvironment): [SetAppSettingsForApplicationEnvironment, SetCommandValues<ISetAppSettingsForApplicationEnvironment>, ClearCommandValues] {
        return useCommand<SetAppSettingsForApplicationEnvironment, ISetAppSettingsForApplicationEnvironment>(SetAppSettingsForApplicationEnvironment, initialValues);
    }
}
