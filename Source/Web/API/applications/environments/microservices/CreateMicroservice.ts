/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices');

export interface ICreateMicroservice {
    applicationId?: string;
    environmentId?: string;
    microserviceId?: string;
    name?: string;
}

export class CreateMicroserviceValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        microserviceId: new Validator(),
        name: new Validator(),
    };
}

export class CreateMicroservice extends Command<ICreateMicroservice> implements ICreateMicroservice {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new CreateMicroserviceValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _microserviceId!: string;
    private _name!: string;

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
            'microserviceId',
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
    get name(): string {
        return this._name;
    }

    set name(value: string) {
        this._name = value;
        this.propertyChanged('name');
    }

    static use(initialValues?: ICreateMicroservice): [CreateMicroservice, SetCommandValues<ICreateMicroservice>, ClearCommandValues] {
        return useCommand<CreateMicroservice, ICreateMicroservice>(CreateMicroservice, initialValues);
    }
}
