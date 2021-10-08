const settings = {
  method: "POST",
};

fetch("http://localhost:5000", settings).then(response => {
  const { status } = response;

  console.log({ status });

  response.text().then(content => {
    console.log({ content });
  });
}).catch(error => {
  console.error({ error });
});
