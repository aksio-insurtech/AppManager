/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/applications/commands';
import { Validator } from '@aksio/applications/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/ingresses');

export interface ICreateIngress {
    applicationId?: string;
    environmentId?: string;
    ingressId?: string;
    name?: string;
}

export class CreateIngressValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        ingressId: new Validator(),
        name: new Validator(),
    };
}

export class CreateIngress extends Command<ICreateIngress> implements ICreateIngress {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/ingresses';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new CreateIngressValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _ingressId!: string;
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
    get environmentId(): string {
        return this._environmentId;
    }

    set environmentId(value: string) {
        this._environmentId = value;
        this.propertyChanged('environmentId');
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

    static use(initialValues?: ICreateIngress): [CreateIngress, SetCommandValues<ICreateIngress>, ClearCommandValues] {
        return useCommand<CreateIngress, ICreateIngress>(CreateIngress, initialValues);
    }
}
