/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/deployables/{{deployableId}}/config-files');

export interface ISetConfigFileForDeployable {
    applicationId?: string;
    environmentId?: string;
    microserviceId?: string;
    deployableId?: string;
    name?: string;
    content?: string;
}

export class SetConfigFileForDeployableValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        microserviceId: new Validator(),
        deployableId: new Validator(),
        name: new Validator(),
        content: new Validator(),
    };
}

export class SetConfigFileForDeployable extends Command<ISetConfigFileForDeployable> implements ISetConfigFileForDeployable {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/deployables/{{deployableId}}/config-files';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetConfigFileForDeployableValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _microserviceId!: string;
    private _deployableId!: string;
    private _name!: string;
    private _content!: string;

    constructor() {
        super(Object, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'microserviceId',
            'deployableId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'environmentId',
            'microserviceId',
            'deployableId',
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
    get microserviceId(): string {
        return this._microserviceId;
    }

    set microserviceId(value: string) {
        this._microserviceId = value;
        this.propertyChanged('microserviceId');
    }
    get deployableId(): string {
        return this._deployableId;
    }

    set deployableId(value: string) {
        this._deployableId = value;
        this.propertyChanged('deployableId');
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

    static use(initialValues?: ISetConfigFileForDeployable): [SetConfigFileForDeployable, SetCommandValues<ISetConfigFileForDeployable>, ClearCommandValues] {
        return useCommand<SetConfigFileForDeployable, ISetConfigFileForDeployable>(SetConfigFileForDeployable, initialValues);
    }
}
