/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{environment}/ingresses');

export interface ICreateIngress {
    applicationId?: string;
    ingressId?: string;
    name?: string;
}

export class CreateIngressValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        ingressId: new Validator(),
        name: new Validator(),
    };
}

export class CreateIngress extends Command<ICreateIngress> implements ICreateIngress {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{environment}/ingresses';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new CreateIngressValidator();

    private _applicationId!: string;
    private _ingressId!: string;
    private _name!: string;

    get requestArguments(): string[] {
        return [
            'applicationId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'ingressId',
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
    get ingressId(): string {
        return this._ingressId;
    }

    set ingressId(value: string) {
        this._ingressId = value;
        this.propertyChanged('ingressId');
    }
    get name(): string {
        return this._name;
    }

    set name(value: string) {
        this._name = value;
        this.propertyChanged('name');
    }

    static use(initialValues?: ICreateIngress): [CreateIngress, SetCommandValues<ICreateIngress>] {
        return useCommand<CreateIngress, ICreateIngress>(CreateIngress, initialValues);
    }
}
