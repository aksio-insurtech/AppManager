/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/applications/commands';
import { Validator } from '@aksio/applications/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/config-files');

export interface ISetConfigFileForApplicationEnvironment {
    applicationId?: string;
    environmentId?: string;
    name?: string;
    content?: string;
}

export class SetConfigFileForApplicationEnvironmentValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        name: new Validator(),
        content: new Validator(),
    };
}

export class SetConfigFileForApplicationEnvironment extends Command<ISetConfigFileForApplicationEnvironment> implements ISetConfigFileForApplicationEnvironment {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/config-files';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetConfigFileForApplicationEnvironmentValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _name!: string;
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
            'name',
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
    get name(): string {
        return this._name;
    }

    set name(value: string) {
        this._name = value;
        this.propertyChanged('name');
    }
    get content(): string {
        return this._content;
    }

    set content(value: string) {
        this._content = value;
        this.propertyChanged('content');
    }

    static use(initialValues?: ISetConfigFileForApplicationEnvironment): [SetConfigFileForApplicationEnvironment, SetCommandValues<ISetConfigFileForApplicationEnvironment>, ClearCommandValues] {
        return useCommand<SetConfigFileForApplicationEnvironment, ISetConfigFileForApplicationEnvironment>(SetConfigFileForApplicationEnvironment, initialValues);
    }
}
