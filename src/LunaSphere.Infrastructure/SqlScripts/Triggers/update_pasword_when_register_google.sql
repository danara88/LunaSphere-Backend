--- +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
---
--- Assign a value to the password hash field when user
--- was registered with Google Provider.
---
--- Example: IsGoogle = true, then PasswordHash = ******* 
---
--- +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
CREATE OR REPLACE TRIGGER update_password_when_register_google_trigger
BEFORE INSERT ON "User" FOR EACH ROW
EXECUTE FUNCTION update_password_when_register_google();


CREATE OR REPLACE FUNCTION update_password_when_register_google()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
	
	IF (NEW."IsGoogle" = TRUE) THEN
		NEW."PasswordHash" := '******';
	END IF;

	RETURN NEW;

END;
$function$