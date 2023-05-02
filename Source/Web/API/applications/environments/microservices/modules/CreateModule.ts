/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/modules');

export interface ICreateModule {
    applicationId?: string;
    environmentId?: string;
    microserviceId?: string;
    moduleId?: string;
    moduleId?: string;
    name?: string;
}

export class CreateModuleValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        microserviceId: new Validator(),
        moduleId: new Validator(),
        moduleId: new Validator(),
        name: new Validator(),
    };
}

export class CreateModule extends Command<ICreateModule> implements ICreateModule {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/modules';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new CreateModuleValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _microserviceId!: string;
    private _moduleId!: string;
    private _moduleId!: string;
    private _name!: string;

    constructor() {
        super(Object, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'microserviceId',
            'moduleId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'environmentId',
            'microserviceId',
            'moduleId',
            'moduleId',
            'name',
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
    get microserviceId(): string {
        return this._microserviceId;
    }

    set microserviceId(value: string) {
        this._microserviceId = value;
        this.propertyChanged('microserviceId');
    }
    get moduleId(): string {
        return this._moduleId;
    }

    set moduleId(value: string) {
        this._moduleId = value;
        this.propertyChanged('moduleId');
    }
    get moduleId(): string {
        return this._moduleId;
    }

    set moduleId(value: string) {
        this._moduleId = value;
        this.propertyChanged('moduleId');
    }
    get name(): string {
        return this._name;
    }

    set name(value: string) {
        this._name = value;
        this.propertyChanged('name');
    }

    static use(initialValues?: ICreateModule): [CreateModule, SetCommandValues<ICreateModule>, ClearCommandValues] {
        return useCommand<CreateModule, ICreateModule>(CreateModule, initialValues);
    }
}
