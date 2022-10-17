/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{applicationId}/environments/{environmentId}/microservices/{{microserviceId}}/deployables');

export interface ICreate {
    microserviceId?: string;
    deployableId?: string;
    name?: string;
}

export class CreateValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        microserviceId: new Validator(),
        deployableId: new Validator(),
        name: new Validator(),
    };
}

export class Create extends Command<ICreate> implements ICreate {
    readonly route: string = '/api/applications/{applicationId}/environments/{environmentId}/microservices/{{microserviceId}}/deployables';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new CreateValidator();

    private _microserviceId!: string;
    private _deployableId!: string;
    private _name!: string;

    get requestArguments(): string[] {
        return [
            'microserviceId',
        ];
    }

    get properties(): string[] {
        return [
            'microserviceId',
            'deployableId',
            'name',
        ];
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

    static use(initialValues?: ICreate): [Create, SetCommandValues<ICreate>] {
        return useCommand<Create, ICreate>(Create, initialValues);
    }
}
