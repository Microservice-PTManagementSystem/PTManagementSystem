/*import { NextRequest, NextResponse } from "next/server";

export async function POST(req: NextRequest) {
  const body = await req.json();        // {firstName,lastName,email,password}
  try {
    
    const tokenRes = await fetch(
      `${process.env.KEYCLOAK_BASEURL}/realms/${process.env.KEYCLOAK_REALM}/protocol/openid-connect/token`,
      {
        method: "POST",
        headers: { "Content-Type": "application/x-www-form-urlencoded" },
        body: new URLSearchParams({
          grant_type: "client_credentials",
          client_id: process.env.KEYCLOAK_ADMIN_CLIENT_ID!,
          client_secret: process.env.KEYCLOAK_ADMIN_CLIENT_SECRET!,
        }),
      }
    ).then(r => r.json());

    
    const createRes = await fetch(
      `${process.env.KEYCLOAK_BASEURL}/admin/realms/${process.env.KEYCLOAK_REALM}/users`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${tokenRes.access_token}`,
        },
        body: JSON.stringify({
          username: body.email,
          email: body.email,
          firstName: body.firstName,
          lastName: body.lastName,
          enabled: true,
          credentials: [
            { type: "password", value: body.password, temporary: false },
          ],
        }),
      }
    );
    if (createRes.status !== 201) {
        const errTxt = await createRes.text();
        console.error(
          "Keycloak create‑user error",
          createRes.status,
          errTxt
        );                 
        return NextResponse.json(
          { ok: false, error: `kc ${createRes.status}: ${errTxt}` },
          { status: 500 }
        );
     }

    if (createRes.status === 201) {
      return NextResponse.json({ ok: true });
    }
    const err = await createRes.text();
    return NextResponse.json({ ok: false, error: err }, { status: 400 });
  } catch (e) {
    return NextResponse.json({ ok: false, error: "internal" }, { status: 500 });
  }
}
*/