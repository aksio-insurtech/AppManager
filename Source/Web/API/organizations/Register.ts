/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/organizations');

export interface IRegister {
    id?: string;
    name?: string;
}

export class RegisterValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        id: new Validator(),
        name: new Validator(),
    };
}

export class Register extends Command<IRegister> implements IRegister {
    readonly route: string = '/api/organizations';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new RegisterValidator();

    private _id!: string;
    private _name!: string;

    get requestArguments(): string[] {
        return [
        ];
    }

    get properties(): string[] {
        return [
            'id',
            'name',
        ];
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

    static use(initialValues?: IRegister): [Register, SetCommandValues<IRegister>] {
        return useCommand<Register, IRegister>(Register, initialValues);
    }
}
