/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{environment}');

export interface ICreateMicroservice {
    applicationId?: string;
    microserviceId?: string;
    name?: string;
}

export class CreateMicroserviceValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        microserviceId: new Validator(),
        name: new Validator(),
    };
}

export class CreateMicroservice extends Command<ICreateMicroservice> implements ICreateMicroservice {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{environment}';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new CreateMicroserviceValidator();

    private _applicationId!: string;
    private _microserviceId!: string;
    private _name!: string;

    get requestArguments(): string[] {
        return [
            'applicationId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
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

    static use(initialValues?: ICreateMicroservice): [CreateMicroservice, SetCommandValues<ICreateMicroservice>] {
        return useCommand<CreateMicroservice, ICreateMicroservice>(CreateMicroservice, initialValues);
    }
}