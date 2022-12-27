/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{environmentId}/microservices/{{microserviceId}}/stack');

export interface ISetStack {
    applicationId?: string;
    microserviceId?: string;
    id?: string;
    name?: string;
    displayName?: string;
    shortName?: string;
}

export class SetStackValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        microserviceId: new Validator(),
        id: new Validator(),
        name: new Validator(),
        displayName: new Validator(),
        shortName: new Validator(),
    };
}

export class SetStack extends Command<ISetStack> implements ISetStack {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{environmentId}/microservices/{{microserviceId}}/stack';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetStackValidator();

    private _applicationId!: string;
    private _microserviceId!: string;
    private _id!: string;
    private _name!: string;
    private _displayName!: string;
    private _shortName!: string;

    constructor() {
        super(Object, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'microserviceId',
            'environment',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'microserviceId',
            'id',
            'name',
            'displayName',
            'shortName',
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
    get id(): string {
        return this._id;
    }

    set id(value: string) {
        this._id = value;
        this.propertyChanged('id');
    }
    get name(): string {
        return this._name;
    }

    set name(value: string) {
        this._name = value;
        this.propertyChanged('name');
    }
    get displayName(): string {
        return this._displayName;
    }

    set displayName(value: string) {
        this._displayName = value;
        this.propertyChanged('displayName');
    }
    get shortName(): string {
        return this._shortName;
    }

    set shortName(value: string) {
        this._shortName = value;
        this.propertyChanged('shortName');
    }

    static use(initialValues?: ISetStack): [SetStack, SetCommandValues<ISetStack>, ClearCommandValues] {
        return useCommand<SetStack, ISetStack>(SetStack, initialValues);
    }
}
