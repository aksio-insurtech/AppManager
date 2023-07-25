/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/applications/commands';
import { Validator } from '@aksio/applications/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/settings/mongodb');

export interface ISetMongoDBSettings {
    organizationId?: string;
    publicKey?: string;
    privateKey?: string;
}

export class SetMongoDBSettingsValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        organizationId: new Validator(),
        publicKey: new Validator(),
        privateKey: new Validator(),
    };
}

export class SetMongoDBSettings extends Command<ISetMongoDBSettings> implements ISetMongoDBSettings {
    readonly route: string = '/api/settings/mongodb';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetMongoDBSettingsValidator();

    private _organizationId!: string;
    private _publicKey!: string;
    private _privateKey!: string;

    constructor() {
        super(Object, false);
    }

    get requestArguments(): string[] {
        return [
        ];
    }

    get properties(): string[] {
        return [
            'organizationId',
            'publicKey',
            'privateKey',
        ];
    }

    get organizationId(): string {
        return this._organizationId;
    }

    set organizationId(value: string) {
        this._organizationId = value;
        this.propertyChanged('organizationId');
    }
    get publicKey(): string {
        return this._publicKey;
    }

    set publicKey(value: string) {
        this._publicKey = value;
        this.propertyChanged('publicKey');
    }
    get privateKey(): string {
        return this._privateKey;
    }

    set privateKey(value: string) {
        this._privateKey = value;
        this.propertyChanged('privateKey');
    }

    static use(initialValues?: ISetMongoDBSettings): [SetMongoDBSettings, SetCommandValues<ISetMongoDBSettings>, ClearCommandValues] {
        return useCommand<SetMongoDBSettings, ISetMongoDBSettings>(SetMongoDBSettings, initialValues);
    }
}
